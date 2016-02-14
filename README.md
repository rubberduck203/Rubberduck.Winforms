# Rubberduck.Winforms
A DataAnnotations and Validation framework for Winforms

## Mission Statement

We all have legacy code. 

Rubberduck.Winforms is meant to bridge the gap between Winforms and WPF.

It enables you to:

  1. Use the same models across WPF, ASP.NET-MVC, and WINFORMS.
  2. Refactor your existing Winforms applications into a state where it is relatively simple to move your existing application to WPF.
    Letting you avoid that "big re-write", while still moving forward towards a modern platform.
  3. Get rid of all those ugly validation `MessageBox`es and `ErrorProvider`s by providing a more modern UI.
  4. Get rid of all that ugly boiler plate code by providing simplified API.
  
## Minimum System Requirements

  - .Net Framework v4.0
  - Windows XP
  
## Installing

  TODO - Create Nuget Package and place instructions here
  
## QuickStart

  First, define your Model class and [use DataAnnotations in the normal way][annotations]. Note that your model must [implement INotifyPropertyChanged][INotifyPropertyChanged] in order for the data binding to work correctly. 
  
  [annotations]:https://msdn.microsoft.com/en-us/library/dd901590(VS.95).aspx
  [INotifyPropertyChanged]:https://msdn.microsoft.com/library/ms229614(v=vs.100).aspx
    
    public class Person : INotifyPropertyChanged
    {
        private string _firstName;

        public Person()
        {
            _firstName = String.Empty;
        }

        [Required]
        [RegularExpression("\\D+", ErrorMessage = "Cannot contains numbers")]
        [Display(Name="First Name")]
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                InvokePropertyChanged("FirstName");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void InvokePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

  Instead of inheriting from `Form`, change your form to inherit from the `ModelBoundForm` provided by Rubberduck.Winforms.
  The `ModelBoundForm` requires that you give it a model, much like declaring a `@Model` in a Razor view.
  
    public partial class PersonForm : ModelBoundForm
    {
        public PersonForm()
            :base(model: new Person())
        {
        

  Then, bind your Model's property to the proper input control by creating a new `TextBinding` and specifing the name of the property to be bound to the control.
  
    // Databind the Person.FirstName property to the FirstNameInput TextBox
    FirstNameInput.DataBindings.Add(new TextBinding(this.Model, "FirstName"));
    
  The `ModelBoundForm` can create Labels for you using the `Label.For()` helper.
  
    Register(Label.For(FirstNameInput));
    
  Note that the new Label must be registered with the form in order for it to be displayed.
  
  The `ErrorLabel.For()` helper creates a label to display any validation errors.
  
    Register(ErrorLabel.For(FirstNameInput));
    
  `ErrorLabel`s will automatically display with red text if the model is in an invalid state. The helper automatically wires up an EventHandler for the Input Control's `Validating` event to accomplish this. The text is hidden if the model is re-validated and passes the second time. 
  
  If you wish to validate all of your controls at once, you can do so by calling the `ValidateChildren()` method of the `Form`, triggering the `Validating` method for all of your Input Controls. (This is actually built into Winforms, I can't take credit of this one.)
  
    private void SubmitButton_Click(object sender, EventArgs e)
    {
        if (this.ValidateChildren())
        {
          // do something
        }
    }
    
  ![example ModelBoundForm](http://i.imgur.com/0wl4l4h.png)
  
  There's a full working example project in the [Example directory](https://github.com/ckuhn203/Rubberduck.Winforms/tree/master/Example).
  

  
