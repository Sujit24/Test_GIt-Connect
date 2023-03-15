using System;
using System.Collections.Generic;
using NetTrackModel;
using NetTrackRepository;

namespace NetTrackBiz
{
    internal class ClientBiz
    {
        private List<ClientModel> _clientModelList;
        private ClientRepository _clientRepository;

        //default constructor
        public ClientBiz()
        {
            _clientModelList = new List<ClientModel>();
            _clientRepository = new ClientRepository();
        }

        // this function creates client model
       

        // this function creates client model
        public void UpdateWebSession(ClientModel clientModel)
        {
            _clientRepository = new ClientRepository();
            _clientRepository.UpdateWebSession(clientModel);
        }

        // Get all truck information of a specific customer
       

        public UgsClientModel GetDetailClientInfo(UgsClientModel clientModel)
        {
            return _clientRepository.GetDetailClientInfo(clientModel);
        }

       

        public int SaveClient(UgsClientModel clientModel)
        {
            return _clientRepository.SaveClient(clientModel);
        }

       
        public void getNewZohoContacts(UgsNewZohoContacts m)
        {
           _clientRepository.getNewZohoContacts(m);
        }
    }
}