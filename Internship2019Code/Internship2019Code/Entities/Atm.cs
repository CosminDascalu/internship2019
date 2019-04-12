using System;
using System.Collections.Generic;

namespace Internship2019Code
{
    public class Atm
    {
        private string name;
        private double openingTime;
        private double closingTime;
        private int capacity;
        private int distanceFromUser;
        private Boolean emptyState;
        private Dictionary<Atm, int> distanceToOtherAtms;

        public Atm(string name, double openingTime, double closingTime, int capacity, int distanceFromUser)
        {
            this.name = name;
            this.openingTime = openingTime;
            this.closingTime = closingTime;
            this.capacity = capacity;
            this.distanceFromUser = distanceFromUser;
            this.emptyState = false;
            this.distanceToOtherAtms = distanceToOtherAtms;
        }

        public string getName()
        {
            return this.name;
        }

        public double getOpeningTime()
        {
            return this.openingTime;
        }

        public double getClosingTime()
        {
            return this.closingTime;
        }

        public int getCapacity()
        {
            return this.capacity;
        }

        public void setCapacity(int capacity)
        {
            this.capacity = capacity;
        }

        public int getDistanceFromUser()
        {
            return this.distanceFromUser; 
        }

        public Boolean getEmptyState()
        {
            return this.emptyState;
        }

        public void setEmptyState(Boolean emptyState)
        {
            this.emptyState = emptyState;
        }

        public Dictionary<Atm, Int32> getDistanceToOtherAtms()
        {
            return this.distanceToOtherAtms;
        }

        public void setDistanceToOtherAtms(Dictionary<Atm, int> distanceToOtherAtms)
        {
            this.distanceToOtherAtms = distanceToOtherAtms;
        }
    }
}