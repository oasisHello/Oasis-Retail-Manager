using Caliburn.Micro;
using ORMWPFUI.Library.API;
using ORMWPFUI.Library.Helper;
using ORMWPFUI.Library.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ORMWPFUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndPoint _productEndPoint;
        /// <summary>
        /// Designed to make sure cart items well prepared before assgined to Cart(Property)
        /// </summary>
        private List<UICartItemModel> _cartItems = new List<UICartItemModel>();
        private IConfigHelper _configHelper;
        private ISaleEndPoint _saleEndPoint;
        private StatusInfoViewModel _status;
        private IWindowManager _window;
        private decimal _rate;

        public SalesViewModel(IProductEndPoint productEndPoint, ISaleEndPoint saleEndPoint, 
            IConfigHelper configHelper, StatusInfoViewModel status,IWindowManager window)
        {
            _productEndPoint = productEndPoint;
            _configHelper = configHelper;
            _saleEndPoint = saleEndPoint;
            _status = status;
            _window = window;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            _rate = _configHelper.GetTaxRate() / 100;
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLoaction = WindowStartupLocation.CenterScreen;
                settings.ResizeMode=ResizeMode.NoResize;
                settings.Title = "System Error";

                _status.UpdateMessage("Unauthorized  access", "don't have the permission to access to the sale form");
                _window.ShowDialogAsync(_status,null,null);

            }
        }
        public async Task LoadProducts()
        {
            _productModels = await _productEndPoint.GetAllAsync();
            _productModels.ForEach(p => p.AvailableQuantity = p.QuantityInStock);
            Products = new BindingList<UIProductModel>(_productModels);
        }
        private BindingList<UIProductModel> _products;

        public BindingList<UIProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }
        private UIProductModel _selectedProduct;
        public UIProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }
        private UICartItemModel _selectedCartItem;

        public UICartItemModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        private int _itemQuantity;
        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }
        private BindingList<UICartItemModel> _cart;
        private List<UIProductModel> _productModels;

        public BindingList<UICartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }
        public string SubTotal
        {
            get
            {
                //TODO: Replace with calculation
                return CalculateSubTotal().ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in _cartItems)
            {
                subTotal += (item.QuantityInCart * item.Product.RetailPrice);
            }
            return subTotal;
        }

        public string Tax
        {
            get
            {
                //TODO: Replace with calculation
                return CalculateTax().ToString("C");
            }
        }

        private decimal CalculateTax()
        {
            decimal tax = 0;
            tax = _cartItems
                .Where(c => c.Product.IsTaxable)
                .Sum(c => c.QuantityInCart * c.Product.RetailPrice * _rate);
            //foreach (var item in _cartItems)
            //{
            //    if (item.Product.IsTaxable)
            //    {
            //        tax += (item.QuantityInCart * item.Product.RetailPrice * _rate) / 100;
            //    }
            //}
            return tax;
        }

        public string Total
        {
            get
            {
                //TODO: Replace with calculation
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString("C");
            }
        }
        public bool CanAddToCart
        {
            get
            {
                // Make sure something is selected.
                // Make sure there is an item quantity.
                bool output = false;
                if (ItemQuantity > 0 && SelectedProduct?.AvailableQuantity >= ItemQuantity)
                {
                    output = true;
                }
                return output;
            }
        }
        public void AddToCart()
        {
            UICartItemModel aCartItem = new UICartItemModel()
            {
                Product = SelectedProduct,
                QuantityInCart = ItemQuantity
            };
            //Make sure before Cart items showed on screen, every cart item should be well prepared.
            CheckCartItems(aCartItem);
            Cart = new BindingList<UICartItemModel>(_cartItems); //Details:After many trys, this way can help us populate the data to view.
            Products = new BindingList<UIProductModel>(_productModels);
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        private void CheckCartItems(UICartItemModel cartItem)
        {
            var item = _cartItems.FirstOrDefault(c => c.Product.ProductName.Equals(cartItem.Product.ProductName));
            var product = _productModels.FirstOrDefault(p => p.ProductName.Equals(cartItem.Product.ProductName));
            if (item != null)
            {
                item.QuantityInCart += cartItem.QuantityInCart;
                product.AvailableQuantity -= cartItem.QuantityInCart;

            }
            else
            {
                _cartItems.Add(cartItem);
                product.AvailableQuantity -= cartItem.QuantityInCart;
            }
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                // Make sure something is selected.
                if (ItemQuantity > 0 && SelectedCartItem?.QuantityInCart >= ItemQuantity)
                {
                    output = true;
                }
                return output;
            }
        }

        public void RemoveFromCart()
        {
            var product = _productModels.FirstOrDefault(p => p.ProductName.Equals(SelectedCartItem.Product.ProductName));
            var cart = _cartItems.FirstOrDefault(c => c.Product.ProductName.Equals(SelectedCartItem.Product.ProductName));
            product.AvailableQuantity += ItemQuantity;
            bool flag = SelectedCartItem.QuantityInCart == ItemQuantity;
            if (flag)
            {
                _cartItems.Remove(cart);
            }
            else
            {
                cart.QuantityInCart -= ItemQuantity;
            }
            Cart = new BindingList<UICartItemModel>(_cartItems); //Details:After many trys, this way can help us populate the data to view.
            Products = new BindingList<UIProductModel>(_productModels);
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);

        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                // Maka sure something is in the cart
                if (_cartItems.Count > 0)
                {
                    output = true;
                }
                return output;
            }
        }

        public async Task CheckOut()
        {
            UISaleModel anUISale = new UISaleModel();
            foreach (var item in _cartItems)
            {
                UISaleDetailModel aSaleDetail = new UISaleDetailModel()
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                };

                anUISale.SaleDetails.Add(aSaleDetail);
            }
            await _saleEndPoint.PostSale(anUISale);
            ResetSalesViewModel();
        }

        private void ResetSalesViewModel()
        {
            _cartItems = new List<UICartItemModel>();
            Cart = new BindingList<UICartItemModel>(_cartItems);
            Products = new BindingList<UIProductModel>(_productModels);
            ItemQuantity = 1;
            SelectedProduct = null;
            NotifyOfPropertyChange(() => SelectedCartItem);
            NotifyOfPropertyChange(() => SelectedProduct);
            NotifyOfPropertyChange(() => CanRemoveFromCart);
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);

        }

    }
}
