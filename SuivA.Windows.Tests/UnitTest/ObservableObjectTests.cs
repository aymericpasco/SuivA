using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SuivA.Windows.Tests.UnitTest
{
    [TestClass]
    public class ObservableObjectTests
    {
        [TestMethod]
        public void PropertyChangedEventHandlerIsRaised()
        {
            var obj = new StubObservableObject();

            var raised = false;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.IsTrue(e.PropertyName == "ChangedProperty");
                raised = true;
            };

            obj.ChangedProperty = "Some value";

            if (!raised) Assert.Fail("PropertyChanged was never invoked.");
        }

        private class StubObservableObject : ObservableObject
        {
            private string _changedProperty;

            public string ChangedProperty
            {
                set
                {
                    _changedProperty = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}