using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Internship2019Code
{
    class Logic
    {

        public static List<Atm> getAtmsRoute(int sumToWithdraw, DateTime currentTime, DateTime deadline, List<Atm> atms, List<CreditCard> creditCards, Form results)
        {
            List<Atm> route = new List<Atm>();

            int currentLocation = -1; //user starting point
            int x = 20; int y = 20; //coordinates for displaying result label
            Label newResult; //label to display on form everytime money are withdrawn
            int newHour = 0; int newMinute = 0; //for displaying hours and minutes
            int creditIndex = 0; int atmIndex = 0;//for getting the item we want

            //checking for credit cards availability and removing those that are not longer available considering that a card expires at 00:00 in that day
            for (int i = 0; i < creditCards.Count; i++)
            {
                if (creditCards.ElementAt(i).getExpirationDate() < currentTime)
                {
                    creditCards.RemoveAt(i);
                }
            }

            //sorting the remaining credit cards by fee 
            creditCards.Sort((a, b) => a.getFee().CompareTo(b.getFee()));

            while (sumToWithdraw != 0 && currentTime < deadline)
            {

                //checking the most convenient atm to go to considering the date when the user gets there and the if atm is open
                double possibleNewMostConvenientHour = 0;
                double mostConvenientHour = 24;

                if (currentLocation == -1)
                    for (int i = 0; i < atms.Count; i++)
                    {
                        possibleNewMostConvenientHour = currentTime.Hour + (double)currentTime.Minute / 60 + (double)atms.ElementAt(i).getDistanceFromUser()/60;

                        if (possibleNewMostConvenientHour < mostConvenientHour)
                        {
                            if( atms.ElementAt(i).getOpeningTime() <= possibleNewMostConvenientHour &&
                                possibleNewMostConvenientHour <= atms.ElementAt(i).getClosingTime()
                                ||
                                atms.ElementAt(i).getOpeningTime() >= possibleNewMostConvenientHour &&
                                atms.ElementAt(i).getClosingTime() >= possibleNewMostConvenientHour &&
                                atms.ElementAt(i).getOpeningTime() > atms.ElementAt(i).getClosingTime() )
                            {
                                currentLocation = i;
                                mostConvenientHour = possibleNewMostConvenientHour;
                            }
                            else if( possibleNewMostConvenientHour < atms.ElementAt(i).getOpeningTime() &&
                                     atms.ElementAt(i).getOpeningTime() < atms.ElementAt(i).getClosingTime() )
                            {
                                currentLocation = i;
                                mostConvenientHour = atms.ElementAt(i).getOpeningTime();
                            }
                        } 
                    }
                else
                {
                    Dictionary<Atm, int> dictionary = atms.ElementAt(currentLocation).getDistanceToOtherAtms();
                    for(int i = 0; i < dictionary.Count; i++)
                    {
                        possibleNewMostConvenientHour = currentTime.Hour + (double)currentTime.Minute / 60 + (double)dictionary.ElementAt(i).Value / 60;

                        if (dictionary.ElementAt(i).Key.getEmptyState() == false && possibleNewMostConvenientHour < mostConvenientHour)
                        {
                            if(dictionary.ElementAt(i).Key.getOpeningTime() <= possibleNewMostConvenientHour &&
                                possibleNewMostConvenientHour <= dictionary.ElementAt(i).Key.getClosingTime()
                                ||
                                dictionary.ElementAt(i).Key.getOpeningTime() >= possibleNewMostConvenientHour &&
                                dictionary.ElementAt(i).Key.getClosingTime() >= possibleNewMostConvenientHour &&
                                dictionary.ElementAt(i).Key.getOpeningTime() > dictionary.ElementAt(i).Key.getClosingTime() )
                                {
                                    mostConvenientHour = possibleNewMostConvenientHour;
                                    currentLocation = atms.IndexOf(dictionary.ElementAt(i).Key);
                                }
                                else if (possibleNewMostConvenientHour < dictionary.ElementAt(i).Key.getOpeningTime() &&
                                         dictionary.ElementAt(i).Key.getOpeningTime() < dictionary.ElementAt(i).Key.getClosingTime())
                                {
                                    currentLocation = atms.IndexOf(dictionary.ElementAt(i).Key);
                                    mostConvenientHour = dictionary.ElementAt(i).Key.getOpeningTime();
                                }
                        }
                    }
                }

                //adding atm to route
                route.Add(atms.ElementAt(currentLocation));

                //emptying the atm or in the best case withdrawing the sum
                while(atms.ElementAt(currentLocation).getCapacity() != 0)
                {
                    creditIndex = 0; // we will always check the credit card with smallest fee
                    atmIndex = currentLocation + 1; // atm index to be displayed

                    //getting the new time
                    newHour = (int) mostConvenientHour;
                    newMinute = Convert.ToInt32(60 * (mostConvenientHour - (int) mostConvenientHour));
                    if (newMinute > 60)
                    {
                        newHour++;
                        newMinute -= 60;
                    }
                    currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, newHour, newMinute, 0);

                    if ( creditCards.ElementAt(creditIndex).getAvailableAmount() < creditCards.ElementAt(0).getWithdrawLimit() &&
                         creditCards.ElementAt(creditIndex).getAvailableAmount() <= atms.ElementAt(currentLocation).getCapacity() )
                    {
                        newResult = new Label();
                        if(creditCards.ElementAt(creditIndex).getAvailableAmount() >= sumToWithdraw)
                        {
                            newResult.Text = currentTime.ToString("dd MMMM yyyy HH:mm") + "   Atm " + atmIndex + ": the user withdrawn all the remaining sum"
                                         + " using the card " + creditCards.ElementAt(creditIndex).getName() + " ("
                                         + sumToWithdraw + ")";
                        }
                        else
                        {
                            newResult.Text = currentTime.ToString("dd MMMM yyyy HH:mm") + "   Atm " + atmIndex + ": the user withdrawn all the available amount "
                                         + " from the card " + creditCards.ElementAt(creditIndex).getName() + " ("
                                         + creditCards.ElementAt(creditIndex).getAvailableAmount() + ")";
                        }
                        
                        newResult.Location = new System.Drawing.Point(x, y);
                        y += 20;
                        newResult.AutoSize = true;
                        results.Controls.Add(newResult);

                        atms.ElementAt(currentLocation).setCapacity(atms.ElementAt(currentLocation).getCapacity() - creditCards.ElementAt(creditIndex).getAvailableAmount());
                        sumToWithdraw -= creditCards.ElementAt(creditIndex).getAvailableAmount();
                        creditCards.RemoveAt(creditIndex);

                        if (sumToWithdraw <= 0)
                        {
                            sumToWithdraw = 0;
                            break;
                        }
                    }
                    else if( creditCards.ElementAt(creditIndex).getAvailableAmount() >= creditCards.ElementAt(0).getWithdrawLimit() &&
                             creditCards.ElementAt(creditIndex).getWithdrawLimit() <= atms.ElementAt(currentLocation).getCapacity() )
                    {
                        newResult = new Label();
                        if(creditCards.ElementAt(creditIndex).getWithdrawLimit() >= sumToWithdraw)
                        {
                            newResult.Text = currentTime.ToString("dd MMMM yyyy HH:mm") + "   Atm " + atmIndex + ": the user withdrawn all the remaining sum"
                                         + " using the card " + creditCards.ElementAt(creditIndex).getName() + " ("
                                         + sumToWithdraw + ")";
                        }
                        else
                        {
                            newResult.Text = currentTime.ToString("dd MMMM yyyy HH:mm") + "   Atm " + atmIndex + ": the user reached the withdrawn limit "
                                         + " from the card " + creditCards.ElementAt(creditIndex).getName() + " ("
                                         + creditCards.ElementAt(creditIndex).getWithdrawLimit() + ")";
                        }
                        newResult.Location = new System.Drawing.Point(x, y);
                        y += 20;
                        newResult.AutoSize = true;
                        results.Controls.Add(newResult);

                        atms.ElementAt(currentLocation).setCapacity(atms.ElementAt(currentLocation).getCapacity() - creditCards.ElementAt(creditIndex).getWithdrawLimit());
                        sumToWithdraw -= creditCards.ElementAt(creditIndex).getWithdrawLimit();
                        creditCards.RemoveAt(creditIndex);

                        if (sumToWithdraw < 0)
                        {
                            sumToWithdraw = 0;
                            break;
                        }
                    }
                    else
                    {
                        newResult = new Label();
                        if(atms.ElementAt(currentLocation).getCapacity() >= sumToWithdraw)
                        {
                            newResult.Text = currentTime.ToString("dd MMMM yyyy HH:mm") + "   Atm " + atmIndex + ": the user withdrawn all the remaining sum"
                                         + " using the card " + creditCards.ElementAt(creditIndex).getName() + " ("
                                         + sumToWithdraw + ")";
                        }
                        else
                        {
                            newResult.Text = currentTime.ToString("dd MMMM yyyy HH:mm") + "   Atm " + atmIndex + ": the user reached the atm capacity "
                                         + " using the card " + creditCards.ElementAt(creditIndex).getName() + " ("
                                         + atms.ElementAt(currentLocation).getCapacity() + ")";
                        }
                        newResult.Location = new System.Drawing.Point(x, y);
                        y += 20;
                        newResult.AutoSize = true;
                        results.Controls.Add(newResult);

                        sumToWithdraw -= atms.ElementAt(currentLocation).getCapacity();
                        if (sumToWithdraw <= 0)
                        {
                            sumToWithdraw = 0;
                            break;
                        }

                        creditCards.ElementAt(creditIndex).setAvailableAmount(creditCards.ElementAt(creditIndex).getAvailableAmount() - atms.ElementAt(currentLocation).getCapacity());
                        atms.ElementAt(currentLocation).setCapacity(0);
                        atms.ElementAt(currentLocation).setEmptyState(true);
                    }
                }
            }

            return route;
        }
    }
}