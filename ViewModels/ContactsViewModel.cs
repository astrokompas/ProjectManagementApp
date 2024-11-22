using System.Collections.ObjectModel;
using System.Windows.Input;
using ProjectManagementApp.Models;
using ProjectManagementApp.Repositories;
using ProjectManagementApp.Windows;
using ProjectManagementApp.Commands;
using System.Windows;

namespace ProjectManagementApp.ViewModels
{
    public class ContactsViewModel : BaseViewModel
    {
        private readonly IContactRepository _contactRepository;
        private ObservableCollection<Contact> _contacts;

        public ObservableCollection<Contact> Contacts
        {
            get => _contacts;
            set => SetProperty(ref _contacts, value);
        }

        public ICommand AddContactCommand { get; }
        public ICommand EditContactCommand { get; }
        public ICommand DeleteContactCommand { get; }

        public ContactsViewModel(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
            Contacts = new ObservableCollection<Contact>();

            AddContactCommand = new RelayCommand(ExecuteAddContact);
            EditContactCommand = new RelayCommand<Contact>(ExecuteEditContact);
            DeleteContactCommand = new RelayCommand<Contact>(ExecuteDeleteContact);

            LoadContacts();
        }

        private async void LoadContacts()
        {
            var contacts = await _contactRepository.GetAllContactsAsync();
            Contacts = new ObservableCollection<Contact>(contacts);
        }

        private async void ExecuteAddContact()
        {
            var dialog = new ContactDialog();
            var viewModel = new ContactDialogViewModel("Dodaj kontakt");

            dialog.DataContext = viewModel;
            var result = dialog.ShowDialog();

            if (result == true && !string.IsNullOrWhiteSpace(viewModel.Email))
            {
                try
                {
                    var newContact = await _contactRepository.AddContactAsync(new Contact
                    {
                        Email = viewModel.Email
                    });
                    Contacts.Add(newContact);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private async void ExecuteEditContact(Contact contact)
        {
            var dialog = new ContactDialog();
            var viewModel = new ContactDialogViewModel("Edytuj kontakt", contact.Email);

            dialog.DataContext = viewModel;
            var result = dialog.ShowDialog();

            if (result == true && !string.IsNullOrWhiteSpace(viewModel.Email))
            {
                contact.Email = viewModel.Email;
                await _contactRepository.UpdateContactAsync(contact);
                LoadContacts();
            }
        }

        private async void ExecuteDeleteContact(Contact contact)
        {
            var confirmDialog = new ConfirmationDialog(
                "Usuń kontakt",
                "Czy na pewno chcesz usunąć ten kontakt?");

            if (confirmDialog.ShowDialog() == true)
            {
                await _contactRepository.DeleteContactAsync(contact.Id);
                Contacts.Remove(contact);
            }
        }
    }
}