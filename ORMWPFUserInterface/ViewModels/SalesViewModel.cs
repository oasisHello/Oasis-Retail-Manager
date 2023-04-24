using Caliburn.Micro;
using ORMWPFUI.Library.API;
using ORMWPFUI.Library.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndPoint _productEndPoint;

        public SalesViewModel(IProductEndPoint productEndPoint)
        {
            _productEndPoint = productEndPoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }
        public async Task LoadProducts()
        {
            _productModels = await _productEndPoint.GetAll();
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
        /// <summary>
        /// Designed to make sure cart items well prepared before assgined to Cart
        /// </summary>
        private List<UICartItemModel> _cartItems = new List<UICartItemModel>();
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
                decimal subTotal = 0;
                foreach (var item in _cartItems)
                {
                    subTotal += (item.QuantityInCart * item.Product.RetailPrice);
                } 
                return subTotal.ToString("C");
            }
        }
        public string Tax
        {
            get
            {
                //TODO: Replace with calculation
                return $"0.00";
            }
        }
        public string Total
        {
            get
            {
                //TODO: Replace with calculation
                return $"0.00";
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

                return output;
            }
        }

        public void CheckOut()
        {

        }


    }
}
