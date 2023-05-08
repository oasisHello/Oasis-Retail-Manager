using Caliburn.Micro;
using ORMWPFUI.Library.API;
using ORMWPFUI.Library.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ORMWPFUI.ViewModels
{
    public class UserRoleViewModel : Screen
    {
        private StatusInfoViewModel _status;
        private IWindowManager _window;
        private IUserRoleEndpoint _userRoleEndpoint;
        private List<UIUserRoleModel> _usersInfoList;

        private BindingList<UIUserRoleModel> _usersInfo;
        public BindingList<UIUserRoleModel> UsersInfo
        {
            get
            {
                return _usersInfo;
            }

            set
            {
                _usersInfo = value;
                NotifyOfPropertyChange(() => UsersInfo);
            }
        }
        private UIUserRoleModel _selectedUser;
      
        public UIUserRoleModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                SelectedUserName = _selectedUser.EMail;
                var allRoles= _allRoles.Values.ToList();
                var currentRoles = _selectedUser.Roles.Select(x => x.Value).ToList();
                CurrentRoles= new BindingList<string>(currentRoles);
                AvailableRoles = new BindingList<string>(allRoles.Except(currentRoles).ToList());
                NotifyOfPropertyChange(()=>AvailableRoles);
                NotifyOfPropertyChange(() => CurrentRoles);
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }
        public string SelectedRoleToRemove
        {
            get
            {
                return _selectedRoleToRemove;
            }
            set
            {
                _selectedRoleToRemove = value;
            }
        }
        private string _selectedRoleToAdd;

        public string SelectedRoleToAdd
        {
            get { return _selectedRoleToAdd; }
            set { _selectedRoleToAdd = value; }
        }

        public async Task AddSelectedRole()
        {
            if(_selectedRoleToAdd != null)
            {
                await _userRoleEndpoint.AddRole(new UIUserRolePairModel { UserId = SelectedUser.Id, RoleName = SelectedRoleToAdd });
                CurrentRoles.Add(_selectedRoleToAdd);
                AvailableRoles.Remove(_selectedRoleToAdd);
            }
        }
        public async void RemoveSelectedRole()
        {

            if(_selectedRoleToRemove != null)
            { 
               // await _userRoleEndpoint.RemoveRole(SelectedUser.Id,SelectedRoleToRemove);
                AvailableRoles.Add(SelectedRoleToRemove);
                CurrentRoles.Remove(SelectedRoleToRemove);
            }

        }

        public string _selectedUserName;
        private Dictionary<string, string> _allRoles;
        private string _selectedRoleToRemove;

        public string SelectedUserName
        {
            get
            {
                return _selectedUserName;
            }
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }
        public BindingList<string> CurrentRoles { get;set; }
        public BindingList<string> AvailableRoles { get; set; }

        public UserRoleViewModel(StatusInfoViewModel status, IWindowManager window, IUserRoleEndpoint userRoleEndpoint)
        {
            _status = status;
            _window = window;
            _userRoleEndpoint = userRoleEndpoint;
            _usersInfoList = new List<UIUserRoleModel>();
            UsersInfo= new BindingList<UIUserRoleModel>(_usersInfoList);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            //try
            //{
            await LoadUserInfos();
            await LoadAllRoles();
            //}
            //catch (Exception ex)
            //{
            //dynamic settings = new ExpandoObject();
            //settings.WindowStartupLoaction = WindowStartupLocation.CenterScreen;
            //settings.ResizeMode = ResizeMode.NoResize;
            //settings.Title = "System Error";
            //_status.UpdateMessage("Unauthorized  access", "don't have the permission to access to the sale form");
            //await _window.ShowDialogAsync(_status, null, null);
            //await TryCloseAsync();

        
            }
        private async Task LoadUserInfos()
        {
            _usersInfoList = await _userRoleEndpoint.GetUserRoles();
            UsersInfo=  new BindingList<UIUserRoleModel>(_usersInfoList);

        }
        private async Task LoadAllRoles()
        {
            _allRoles = await _userRoleEndpoint.GetAllRoles();
        }
    }
}
