using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Internship2019Code
{
    public partial class ResultsForm : Form
    {
        Atm atm1, atm2, atm3, atm4;
        CreditCard silver, gold, platinum;
        DateTime currentTime, deadline;
        List<CreditCard> creditCards;
        List<Atm> atms;
        int sumToWithdraw = 7500;

        public ResultsForm()
        {
            InitializeComponent();

            currentTime = DateTime.Parse("19.03.2019 11:30");
            deadline = DateTime.Parse("19.03.2019 14:00");
            creditCards = new List<CreditCard>();
            atms = new List<Atm>();

            //openingTime and closingTime were considered double, so that it would be easier to convert to hours (half an hour will be consider 0.5 and so on)
            atm1 = new Atm("atm1", 12, 18, 5000, 5);
            atm2 = new Atm("atm2", 10, 17, 5000, 60);
            atm3 = new Atm("atm3", 22, 12, 5000, 30);
            atm4 = new Atm("atm4", 17, 1, 5000, 45);
            atm1.setDistanceToOtherAtms(new Dictionary<Atm, int>() { { atm2, 40 }, { atm4, 45 } });
            atm2.setDistanceToOtherAtms(new Dictionary<Atm, int>() { { atm3, 15 } });
            atm3.setDistanceToOtherAtms(new Dictionary<Atm, int>() { { atm1, 40 }, { atm4, 15 } });
            atm4.setDistanceToOtherAtms(new Dictionary<Atm, int>() { { atm2, 30 } });
            atms.Add(atm1);
            atms.Add(atm2);
            atms.Add(atm3);
            atms.Add(atm4);

            silver = new CreditCard("silver", 0.2, 4500, DateTime.Parse("23.05.2020"), 20000);
            gold = new CreditCard("gold", 0.1, 3000, DateTime.Parse("15.08.2018"), 25000);
            platinum = new CreditCard("platinum", 0, 4000, DateTime.Parse("20.03.2019"), 3000);
            creditCards.Add(silver);
            creditCards.Add(gold);
            creditCards.Add(platinum);

            //this method will return the route
            //also, this method displays content to the resultsForm containing all the information you need
            atms = Logic.getAtmsRoute(sumToWithdraw, currentTime, deadline, atms, creditCards, this);
        }

        //method used to send the results to Program class
        public List<Atm> getRoute()
        {
            return atms;
        }
    }
}