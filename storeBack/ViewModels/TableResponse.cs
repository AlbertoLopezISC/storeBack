﻿namespace storeBack.ViewModels
{
    public class TableResponse<T>
    {
        public int Count {  get; set; }
        public IEnumerable<T> Results { get; set; }

    }
}
