﻿using System;
using System.Collections.Generic;
using System.Linq;
using Spinit.Wpc.Synologen.Core.Domain.Model.ShopDetails;
using Spinit.Wpc.Synologen.UI.Mvc.Site.Models;

namespace Spinit.Wpc.Synologen.UI.Mvc.Site.App.ViewModelParsers
{
    public class ShopViewModelParserService
    {
        public ShopListView ParseShops(IEnumerable<Shop> shops)
        {
            var parsedShops = shops.Select(ParseShop);
            return new ShopListView {Shops = parsedShops};
        }

        public SearchShopView ParseShops(IEnumerable<Shop> shops, string search)
        {
            var parsedShops = shops.Select(ParseShop);
            return new SearchShopView
            {
                NrOfResults = parsedShops.Count(),
                Search = search,
                Shops = parsedShops
            };
        }

        public ShopListItem ParseShop(Shop shop)
        {
            var address = "";
            var city = "";
            var provides = "";
            if (shop.Address != null)
            {
                address = String.Format("{0} {1}", shop.Address.AddressLineOne, shop.Address.AddressLineTwo);
                city = shop.Address.City;
            }

            if (shop.Connections != null && shop.Connections.Count() > 0)
            {
                provides = shop.Connections.Where(x => x.ShopEquipment != null).Aggregate(provides, (current, connection) => current + String.Format("<em>{0}</em>, ", connection.ShopEquipment.Name));

                if (provides.Length > 0)
                {
                    provides = provides.Substring(0, provides.Length - 2);
                }
            }

            return new ShopListItem
            {
                Description = shop.Description,
                Email = shop.Email,
                HomePage = shop.Url,
                Id = shop.Id,
                Latitude = shop.Coordinates != null ? shop.Coordinates.Latitude : 0,
                Longitude = shop.Coordinates != null ? shop.Coordinates.Longitude : 0,
                Map = shop.MapUrl,
                Name = shop.Name,
                StreetAddress = address,
                Telephone = shop.Phone,
                City = city,
                Provides = provides
            };
        }
    }
}