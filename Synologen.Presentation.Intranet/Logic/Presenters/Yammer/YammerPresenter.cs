﻿using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Spinit.Wpc.Synologen.Business;
using Spinit.Wpc.Synologen.Business.Domain.Interfaces;
using Spinit.Wpc.Synologen.Core.Domain.Model.Yammer;
using Spinit.Wpc.Synologen.Presentation.Intranet.Code.Yammer;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Views.Yammer;
using Spinit.Wpc.Synologen.Presentation.Intranet.Models.Yammer;
using WebFormsMvp;

namespace Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Presenters.Yammer
{
    public class YammerPresenter : Presenter<IYammerView>
    {
        private string _email, _password, _clientId, _network;

        private const int MaxMessagesToFetchFromYammer = 20;

        private readonly IYammerService _service;

        public YammerPresenter(IYammerView view, IYammerService service) : base(view)
        {
            _service = service;

            InitSettings();

            View.Load += View_Load;
        }

        public void View_Load(object sender, EventArgs e)
        {
            if (LivingCookiesExist())
            {
                _service.CookieContainer = View.State["YammerCookies"] as CookieContainer;
            }
            else
            {
				try
				{
					_service.Authenticate(_network, _clientId, _email, _password);
					if (View.State != null)
					{
						View.State["YammerCookies"] = _service.CookieContainer;
					}
				}
				catch(Exception)
				{
					GetFromCache();
				}
            }

			try
			{
				View.Model = GetMessages();
				Cache["messages"] = View.Model;
			}
			catch (Exception)
			{
				GetFromCache();
			}
        }

    	private void GetFromCache()
    	{
    		if (Cache["messages"] != null && Cache["messages"] is YammerListModel)
    		{
    			View.Model = Cache["messages"] as YammerListModel;
    		}
    	}

    	public override void ReleaseView()
        {
            View.Load -= View_Load;
        }

        private YammerListModel GetMessages()
        {
            var objects = GetJsonObjects();

            if (objects == null || objects.messages.Count() == 0)
            {
                _service.Authenticate(_network, _clientId, _email, _password);
                objects = GetJsonObjects();
            }

            return objects == null ?
                new YammerListModel { Messages = Enumerable.Empty<YammerListItem>() } :
                new YammerListModel { Messages = YammerParserService.Convert(objects, _service.FetchImage) };
        }

        private JsonMessageModel GetJsonObjects()
        {
            if (View.NumberOfMessages < 0)
            {
                View.NumberOfMessages = Int16.MaxValue;
            }

            var json = _service.GetJson(View.NumberOfMessages, View.Threaded, View.NewerThan);
            var objects = JsonConvert.DeserializeObject<JsonMessageModel>(json);

            if (objects == null)
                return objects;

            objects.RemoveMessagesWithIdLessOrEqualTo(View.NewerThan);

            if (View.ExcludeJoins)
            {
                int prevOldestId = -1;
                while (objects.messages.Count(x => YammerParserService.IsNotJoinMessage(x.body)) < View.NumberOfMessages)
                {
                    var ids = objects.messages.Where(x => x.id > View.NewerThan);
                    int oldestId = ids.Count() > 0 ? ids.Min(x => x.id) : 0;
                    if (oldestId == prevOldestId)
                    {
                        break;
                    }

                    json = _service.GetJson(MaxMessagesToFetchFromYammer, View.Threaded, View.NewerThan, oldestId);
                    var newObjects = JsonConvert.DeserializeObject<JsonMessageModel>(json);
                    newObjects.RemoveMessagesWithIdLessOrEqualTo(View.NewerThan);
                    objects.messages = objects.messages.Concat(newObjects.messages).ToArray();
                    objects.references = objects.references.Concat(newObjects.references).ToArray();

                    prevOldestId = oldestId;
                }
            }

            var messages = View.ExcludeJoins ? objects.messages.Where(x => YammerParserService.IsNotJoinMessage(x.body)).ToList() : objects.messages.ToList();
            objects.messages = messages.GetRange(0, Math.Min(messages.Count, View.NumberOfMessages)).ToArray();
            return objects;
        }

        private bool LivingCookiesExist()
        {
            var cookies = GetCookies("https://www.yammer.com");

            if (cookies.Count > 0)
            {
                if (cookies.Cast<Cookie>().Any(cookie => cookie.Expired))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private CookieCollection GetCookies(string uri)
        {
            if (View.State != null && View.State["YammerCookies"] is CookieContainer)
            {
                var allCookies = View.State["YammerCookies"] as CookieContainer;
                if (allCookies != null)
                {
                    return allCookies.GetCookies(new Uri(uri));
                }
            }
            return new CookieCollection();
        }

        private void InitSettings()
        {
            try
            {
                _email = Globals.YammerEmailAccount;
                _password = Globals.YammerPassword;
                _clientId = Globals.YammerClientId;
                _network = Globals.YammerNetwork;
            }
            catch (Exception)
            {
                _email = _password = _clientId = _network = String.Empty;
            }
        }
    }
}
