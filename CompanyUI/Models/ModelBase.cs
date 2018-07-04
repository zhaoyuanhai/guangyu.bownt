using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyUI.Models
{
    public class ModelBase
    {
        public ModelBase() { }

        public ModelBase(object obj)
        {
            this.LoadProp(obj);
        }

        public void LoadProp(object obj)
        {
            obj.GetType().GetProperties().ToList().ForEach(a =>
            {
                var p = this.GetType().GetProperty(a.Name);
                if (p != null)
                {
                    p.SetValue(this, a.GetValue(obj));
                }
            });
        }
    }
}