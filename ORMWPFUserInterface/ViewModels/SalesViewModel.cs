using Caliburn.Micro;
using ORMWPFUI.Library.API;
using ORMWPFUI.Library.Helper;
using ORMWPFUI.Library.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
        private decimal _rate;

        public SalesViewModel(IProductEndPoint productEndPoint,ISaleEndPoint saleEndPoint, IConfigHelper configHelper)
        {
            _productEndPoint = productEndPoint;
            _configHelper= configHelper;
            _saleEndPoint = saleEndPoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            _rate = _configHelper.GetTaxRate()/100;
            await LoadProducts();
        }
        public async Task LoadProducts()
        {
            _productModels = await _productEndPoint.GetAllAsync();
            _productModels.ForEach(p => p.Available = p.QuantityInStock);
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
        private UIProductModel _selectedProducts;
        public UIProductModel SelectedProducts
        {
            get { return _selectedProducts; }
            set
            {
                _selectedProducts = value;
                NotifyOfPropertyChange(() => SelectedProducts);
                NotifyOfPropertyChange(() => CanAddToCart);
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
            tax=_cartItems
                .Where(c => c.Product.IsTaxable)
                .Sum(c => c.QuantityInCart*c.Product.RetailPrice*_rate) ;
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
                decimal total = CalculateSubTotal()+CalculateTax();
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
                if (ItemQuantity > 0 && SelectedProducts?.Available > ItemQuantity)
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
                Product = SelectedProducts,
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
        }

        private void CheckCartItems(UICartItemModel cartItem)
        { 
            var item=_cartItems.FirstOrDefault(c => c.Product.ProductName.Equals(cartItem.Product.ProductName));
            var product = _productModels.FirstOrDefault(p => p.ProductName.Equals(cartItem.Product.ProductName));
            if (item!=null)
            {
                item.QuantityInCart += cartItem.QuantityInCart;
                product.Available -= cartItem.QuantityInCart;

            }
            else
            {
                _cartItems.Add(cartItem);
                product.Available -= cartItem.QuantityInCart;
            }
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected.

                return output;

            }
        }

        public void RemoveFromCart()
        {
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                // Maka sure something is in the cart
                if(_cartItems.Count > 0)
                {
                    output= true;
                }
                return output;
            }
        }

        public async Task CheckOut()
        {
            UISaleModel anUISale= new UISaleModel();
            foreach(var item in _cartItems) 
            {
                UISaleDetailModel aSaleDetail = new UISaleDetailModel()
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                };

                anUISale.SaleDetails.Add(aSaleDetail);
            }
                await _saleEndPoint.PostSale(anUISale);
        }

    }
}
