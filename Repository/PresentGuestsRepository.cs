﻿using Project.Model;
using Project.Observer;
using Project.RepositoryInterfaces;
using Project.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.Repository
{
    public class PresentGuestsRepository : ISubject, IPresentGuestsRepository
    {

        private const string FilePath = "../../../Resources/Data/presentguests.csv";

        private readonly Serializer<PresentGuests> serializer;

        private readonly List<IObserver> _observers;

        private List<PresentGuests> presentGuests;



        public PresentGuestsRepository()
        {
            serializer = new Serializer<PresentGuests>();
            presentGuests = serializer.FromCSV(FilePath);
            _observers = new List<IObserver>();
        }

        private void SaveInFile()
        {
            serializer.ToCSV(FilePath, presentGuests);
        }


        public void Add(PresentGuests presentGuest)
        {

            presentGuests.Add(presentGuest);
            SaveInFile();
            NotifyObservers();

        }

        public void ClearPresents()
        {
            presentGuests.Clear();
            SaveInFile();
            NotifyObservers();
        }

        public List<PresentGuests> GetAll()
        {
            return presentGuests;
        }

        public List<int> GetAllGuestIds(int appointmentid)
        {
            List<int> guestIds = new List<int>();
            foreach(PresentGuests presentGuest in presentGuests)
            {
                if(presentGuest.AppointmentId == appointmentid)
                {
                    guestIds.Add(presentGuest.GuestId);
                }
                
            }
            return guestIds;
        }


        public List<PresentGuests> GetByAppointmentId(int id)
        {
            List<PresentGuests> filteredPresentGuests = new List<PresentGuests>();

            foreach (PresentGuests presentGuest in presentGuests)
            {
                if(presentGuest.AppointmentId == id)
                {
                    filteredPresentGuests.Add(presentGuest);
                }
            }
            return filteredPresentGuests;
        }

        public List<User> GetUserByAppointmentId(int id)
        {
            List<User> filteredPresentUsers = new List<User>();
            UserRepository userRepo = new UserRepository();


            foreach (PresentGuests presentGuest in presentGuests)
            {
                if (presentGuest.AppointmentId == id)
                {
                    filteredPresentUsers.Add(userRepo.GetById(presentGuest.GuestId));
                }
            }
            return filteredPresentUsers;
        }

        public int GetBoardingPointByGuestIdAndAppointmentId(int guestId,int appointmentId)
        {
             PresentGuests present = presentGuests.Find(v => v.GuestId == guestId && v.AppointmentId == appointmentId);
            if (present == null) 
            {
                return -1;
            }
            else
            {
                return present.TourPointId;
            }
             
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }


    }
}
