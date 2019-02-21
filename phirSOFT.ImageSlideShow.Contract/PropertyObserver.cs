using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow
{
    public class PropertyObserver<TObject> where TObject : class, INotifyPropertyChanged
    {
        
        public static PropertyObserver<TTarget> ObserveProptery<TTarget, TProperty>(this TTarget target, Expression<Func<TTarget, TProperty>> property, Action<TProperty> action) where TTarget : INotifyPropertyChanged
        {
            if(!(property is MemberExpression memberExpression && memberExpression.Member is PropertyInfo propertyInfo))
                throw new ArgumentException();


            target.PropertyChanged += (sender, e) =>
            {
                if(e.PropertyName == propertyInfo.Name)
                {
                    action((TProperty) propertyInfo.GetValue(target));
                }
            };
            return target;
        }
    }
}
