using System;

namespace Internship2019Code
{
    class CreditCard
    {
        private string name;
        private double fee;
        private int withdrawLimit;
        private DateTime expirationDate;
        private int availableAmount;

        public CreditCard(string name, double fee, int withdrawLimit, DateTime expirationDate, int availableAmount)
        {
            this.name = name;
            this.fee = fee;
            this.withdrawLimit = withdrawLimit;
            this.expirationDate = expirationDate;
            this.availableAmount = availableAmount;
        }

        public string getName()
        {
            return this.name;
        }

        public double getFee()
        {
            return this.fee;
        }

        public int getWithdrawLimit()
        {
            return this.withdrawLimit;
        }

        public DateTime getExpirationDate()
        {
            return this.expirationDate;
        }

        public int getAvailableAmount()
        {
            return this.availableAmount;
        }

        public void setAvailableAmount(int availableAmount)
        {
            this.availableAmount = availableAmount;
        }
    }
}
