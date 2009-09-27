namespace MvcContrib.UI.InputBuilder
{
    public class ModelProperty<T>:InputModelProperty
    {
    	public new T Value
    	{
    		get
    		{
    			return (T)base.Value;
    		} 
			set
			{
				base.Value = value;
			}
    	}

    }
}