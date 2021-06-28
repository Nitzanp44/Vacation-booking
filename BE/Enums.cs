using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Enums
    {
        public enum Area {הכל, מרכז,  ירושלים, צפון, דרום }

        public enum SubArea { גליל, גולן, חיפה, בקעה, שפלה, ירושלים, בנימין, חברון, ים_המלח, אילת, מצפה_רמון, ערבה, באר_שבע, תל_אביב, נתניה, שומרון, אשדוד }

       public enum SubAreaNorth { גליל ,גולן, חיפה}
       public enum SubAreaJerus { בקעה, שפלה, ירושלים, בנימין, חברון, ים_המלח}
       public enum SubAreaSouth { אילת, מצפה_רמון, ערבה, באר_שבע}
       public enum SubAreaCenter { תל_אביב, נתניה, שומרון, אשדוד}

        public enum KindOfVication { צימר, מלון, חאן, קמפינג}
        public enum StatusOrder { טרם_טופל, נשלח_מייל, נסגר_מחוסר_היענות, נסגר_בהיענות_של_הלקוח}
        public enum StatusClient { פתוחה, נסגרה_עסקה_דרך_האתר, נסגרה_כי_פג_תוקפה}
        public enum Option { הכרחי, אפשרי, לא_מעוניין}
        //public enum Price { עד_100, עד_500, עד_1000, יותר_מ_1000 }
    }
}
