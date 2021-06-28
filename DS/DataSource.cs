using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DS
{
    public class DataSource
    {
       public static List<BE.Order> orders = new List<BE.Order>()
        {
            new BE.Order()
            {
                Status=BE.Enums.StatusOrder.נסגר_בהיענות_של_הלקוח,
                CreateDate= DateTime.Now,
                numOrderKey=BE.Configuration.OrderKey++,
                numHostingUnitKey=12345678,
                numGuestRequest=98,
                OrderDate=DateTime.Now.AddDays(30)
            },
            new BE.Order()
            {
                Status=BE.Enums.StatusOrder.נסגר_בהיענות_של_הלקוח,
                CreateDate= new DateTime(2020, 3, 5),
                numOrderKey=BE.Configuration.OrderKey++,
                numHostingUnitKey=10001,
                numGuestRequest=99,
                OrderDate=new DateTime(2020, 4, 2)
            },
            // new BE.Order()
            //{
            //    Status=BE.Enums.StatusOrder.נסגר_בהיענות_של_הלקוח,
            //    CreateDate= new DateTime(2020, 11, 9),
            //    numOrderKey=BE.Configuration.OrderKey++,
            //    numHostingUnitKey=BE.Configuration.HostingUnitKey++,
            //    numGuestRequest=103,
            //    OrderDate=new DateTime(2020, 12, 28)
            //},
        };



        public static List<BE.GuestRequest> guestRequests = new List<BE.GuestRequest>()
        {
            new BE.GuestRequest()
            {
                 numGuestRequest = 99,
                 PrivateName="Nitzan",
                 FamilyName="Peleg",
                 MailAddress="Nitzanp44@gmail.com",
                 Status=BE.Enums.StatusClient.נסגרה_עסקה_דרך_האתר,
                 RegistrationDate=new DateTime(2020, 10, 28),
                 EntryDate=new DateTime(2020, 11, 28),
                 ReleaseDate=new DateTime(2020, 12, 3),
                 Area=BE.Enums.Area.צפון,
                 SubArea=BE.Enums.SubArea.גולן,
                 Type=BE.Enums.KindOfVication.מלון,
                 Price=1011,
                 Adults=2,
                 Children=0,
                 Pool=BE.Enums.Option.אפשרי,
                 Jacuzzi=BE.Enums.Option.הכרחי,
                 Garden=BE.Enums.Option.אפשרי,
                 ChildrensAttractions=BE.Enums.Option.לא_מעוניין,
                 Breakfast=BE.Enums.Option.הכרחי
            },

            new BE.GuestRequest()
            {
                 numGuestRequest = 98,
                 PrivateName="reut",
                 FamilyName="levenberg",
                 MailAddress="reut.baum@gmail.com",
                 Status=BE.Enums.StatusClient.פתוחה,
                 RegistrationDate=new DateTime(2020, 1, 5),
                 EntryDate=new DateTime(2020, 2, 12),
                 ReleaseDate=new DateTime(2020, 2, 20),
                 Area=BE.Enums.Area.דרום,
                 SubArea=BE.Enums.SubArea.אילת,
                 Type=BE.Enums.KindOfVication.מלון,
                 Price=555,
                 Adults=2,
                 Children=1,
                 Pool=BE.Enums.Option.הכרחי,
                 Jacuzzi=BE.Enums.Option.אפשרי,
                 Garden=BE.Enums.Option.לא_מעוניין,
                 ChildrensAttractions=BE.Enums.Option.לא_מעוניין,
                 Breakfast=BE.Enums.Option.הכרחי
            },
        };


        public static List<BE.HostingUnit> hostingUnits = new List<BE.HostingUnit>()
        {
            new BE.HostingUnit()
            {
                numHostingUnitKey=10001,
                Owner= new BE.Host()
                {
                    HostKey=10,
                    PrivateName="Nadav",
                    FamilyName ="Levi",
                    FhoneNumber=0526762502,
                    MailAddress="Nadav@gmail.com",
                    BankAccuontHost=new BE.BankAccount()
                    {
                        BankNumber=11,
                        BankName="דיסקונט",
                        BranchNumber=123,
                        BranchAddress="כנפי נשרים 3",
                        BranchCity="קצרין",

                    },
                    CollectionClearance=true,
                    numOfUnit=1
                },
                HostingUnitName="נופי גולן",
                diary=new bool[12,31],
                Area=BE.Enums.Area.צפון,
                SubArea=Enums.SubArea.גולן,
                Address="השקד 14 חיספין",
                price=100,
                Pool=true,
                Jacuzzi=true,
                Garden=true,
                ChildrensAttractions = true,
                Breakfast=true,
                Capacity =7,
                TypeOfHost=BE.Enums.KindOfVication.צימר
            },
            new BE.HostingUnit()
            {
                numHostingUnitKey=12345678,
                Owner= new BE.Host()
                {
                    HostKey=11,
                    PrivateName="יאיר",
                    FamilyName ="baum",
                    FhoneNumber=0526111502,
                    MailAddress="yair@gmail.com",
                    BankAccuontHost=new BE.BankAccount()
                    {
                        BankNumber=12,
                        BankName="בנק הפועלים",
                        BranchNumber=234,
                        BranchAddress="שדרות רגר 3",
                        BranchCity="באר שבע",

                    },
                    CollectionClearance=true,
                    numOfUnit=1
                },
                HostingUnitName="נופי מדבר",
                diary=new bool[12,31],
                Area=BE.Enums.Area.דרום,
                SubArea=Enums.SubArea.אילת,
                Address="החרוב 12 עומר ",
                price=100,
                Pool=false,
                Jacuzzi=true,
                Garden=true,
                ChildrensAttractions = false,
                Breakfast=true,
                Capacity =3,
                TypeOfHost=BE.Enums.KindOfVication.מלון
            },
        };
    }
}
