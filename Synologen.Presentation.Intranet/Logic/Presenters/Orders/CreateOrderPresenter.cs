using System;
using System.Linq;
using Spinit.Extensions;
using Spinit.Wpc.Synologen.Core.Domain.Model.Orders;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.Criterias.Orders;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.Orders;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.EventArguments.Orders;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Services;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Views.Orders;
using Spinit.Wpc.Synologen.Presentation.Intranet.Models;
using WebFormsMvp;

namespace Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Presenters.Orders
{
    public class CreateOrderPresenter : Presenter<ICreateOrderView>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRoutingService _routingService;
    	private readonly IArticleCategoryRepository _articleCategoryRepository;
    	private readonly IViewParser _viewParser;
    	private readonly IArticleSupplierRepository _articleSupplierRepository;
    	private readonly IArticleTypeRepository _articleTypeRepository;
    	private readonly IOrderCustomerRepository _orderCustomerRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ILensRecipeRepository _lensRecipeRepository;

        public CreateOrderPresenter(
			ICreateOrderView view, 
			IOrderRepository orderRepository, 
			IOrderCustomerRepository orderCustomerRepository, 
            IRoutingService routingService,
			IArticleCategoryRepository articleCategoryRepository,
			IViewParser viewParser,
			IArticleSupplierRepository articleSupplierRepository,
			IArticleTypeRepository articleTypeRepository,
            IArticleRepository articleRepository,
            ILensRecipeRepository lensRecipeRepository
			) : base(view)
        {
            _orderCustomerRepository = orderCustomerRepository;
            _routingService = routingService;
        	_articleCategoryRepository = articleCategoryRepository;
        	_viewParser = viewParser;
        	_articleSupplierRepository = articleSupplierRepository;
        	_articleTypeRepository = articleTypeRepository;
        	_orderRepository = orderRepository;
            _articleRepository = articleRepository;
            _lensRecipeRepository = lensRecipeRepository;
        	View.Load += View_Load;
            View.Submit += View_Submit;
            View.Abort += View_Abort;

            View.SelectedCategory += FillModel;
            View.SelectedArticleType += FillModel;
            View.SelectedSupplier += FillModel;
            View.SelectedArticle += FillModel;
        }

        public void FillModel(object sender, SelectedSomethingEventArgs args)
        {
            if(args.SelectedCategoryId > 0)
            {
                var criteria = new ArticleTypesByCategory(args.SelectedCategoryId);
                var articleTypes = _articleTypeRepository.FindBy(criteria);
                View.Model.ArticleTypes = _viewParser.ParseWithDefaultItem(articleTypes, supplier => new ListItem(supplier.Name, supplier.Id));
            }
            if(args.SelectedArticleTypeId > 0)
            {
                var suppliers = _articleSupplierRepository.GetAll();
                var filteredSuppliers = suppliers.Where(articleSupplier => articleSupplier.Articles.Where(x => x.ArticleType.Id == args.SelectedArticleTypeId).ToList().Count > 0).ToList();
                View.Model.Suppliers = _viewParser.ParseWithDefaultItem(filteredSuppliers, supplier => new ListItem(supplier.Name, supplier.Id));
            }
            if(args.SelectedSupplierId > 0)
            {
                var criteria = new ArticlesBySupplierAndArticleType(args.SelectedSupplierId, args.SelectedArticleTypeId);
                var articles = _articleRepository.FindBy(criteria);
                View.Model.OrderArticles = _viewParser.ParseWithDefaultItem(articles, article => new ListItem(article.Name, article.Id));

                var supplier = _articleSupplierRepository.Get(args.SelectedSupplierId);
                View.Model.ShippingOptions = _viewParser.Parse(supplier.ShippingOptions);
            }
            if(args.SelectedArticleId > 0)
            {
                var article = _articleRepository.Get(args.SelectedArticleId);
                var options = article.Options;
                if (options == null) return;
                View.Model.PowerOptions = _viewParser.FillWithIncrementalValues(options.Power);
                View.Model.DiameterOptions = _viewParser.FillWithIncrementalValues(options.Diameter);
                View.Model.BaseCurveOptions = _viewParser.FillWithIncrementalValues(options.BaseCurve);
                View.Model.AxisOptions = _viewParser.FillWithIncrementalValues(options.Axis);
                View.Model.CylinderOptions = _viewParser.FillWithIncrementalValues(options.Cylinder);
            }

            View.Model.SelectedCategoryId = args.SelectedCategoryId;
            View.Model.SelectedArticleTypeId = args.SelectedArticleTypeId;
            View.Model.SelectedSupplierId = args.SelectedSupplierId;
            View.Model.SelectedArticleId = args.SelectedArticleId;
        }

        public override void ReleaseView()
        {
            View.Submit -= View_Submit;
            View.Load -= View_Load;
			View.SelectedCategory -= FillModel;
            View.SelectedArticle -= FillModel;
            View.SelectedArticleType -= FillModel;
            View.SelectedSupplier -= FillModel;
        }

        public void View_Submit(object o, CreateOrderEventArgs form)
        {
            var article = _articleRepository.Get(form.ArticleId);
            if (article == null) return;

			var lensRecipe = new LensRecipe
            {
                Axis = new EyeParameter { Left = form.LeftAxis, Right = form.RightAxis },
                BaseCurve = new EyeParameter { Left = form.LeftBaseCurve, Right = form.RightBaseCurve },
                Cylinder = new EyeParameter { Left = form.LeftCylinder, Right = form.RightCylinder },
                Diameter = new EyeParameter { Left = form.LeftDiameter, Right = form.RightDiameter },
                Power = new EyeParameter { Left = form.LeftPower, Right = form.RightPower }
            };
            _lensRecipeRepository.Save(lensRecipe);

        	var customerId = HttpContext.Request.Params["customer"].ToInt();
        	var customer = _orderCustomerRepository.Get(customerId);
            var order = new Order
            {
                Article = article,
                LensRecipe = lensRecipe,
                ShippingType = (OrderShippingOption) form.ShipmentOption,
			    Customer = customer
			};
			_orderRepository.Save(order);
            Redirect();
        }

        private void Redirect()                         
        {
            var url = _routingService.GetPageUrl(View.NextPageId);
            HttpContext.Response.Redirect(url);
        }

        public void View_Load(object o, EventArgs eventArgs)
        {
        	var customerIdParameter = HttpContext.Request.Params["customer"];
            var customerId = Convert.ToInt32(customerIdParameter);

        	var categories = _articleCategoryRepository.GetAll();
            var parsedCategories = _viewParser.ParseWithDefaultItem(categories, category => new ListItem(category.Name, category.Id));
            View.Model.Categories = parsedCategories;

            var customer = _orderCustomerRepository.Get(customerId);

            View.Model.CustomerId = customerId;
            View.Model.CustomerName = String.Format("{0} {1}", customer.FirstName, customer.LastName);
        }   

        public void View_Abort(object o, EventArgs eventArgs)
        {
            var url = _routingService.GetPageUrl(View.AbortPageId);
            HttpContext.Response.Redirect(url);
        }

        public void View_Previous(object o, EventArgs eventArgs)
        {
            var url = _routingService.GetPageUrl(View.PreviousPageId);
            HttpContext.Response.Redirect(url);
        }
    }
}