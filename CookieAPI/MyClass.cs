namespace CookieAPI
{
    public class MyClass
    {

        public int? NullableProperty { get; set; }
        public string NonNullableProperty { get; set; }

        //Dictionary<Tuple<int, string>, int> data = new Dictionary<Tuple<int, string>, int>();

        //public int this[int index1, string index2]
        //{
        //    get { 

        //        Tuple<int, string> key = Tuple.Create(index1, index2); 
        //        if(data.TryGetValue(key, out int v))
        //        {
        //            return v;
        //        }
        //        else
        //        {
        //            return -1;
        //        }

        //    }
        //    set {

        //        Tuple<int, string> key = Tuple.Create(index1, index2);
        //        data[key] = value;
        //    }

        //}
    }
}
