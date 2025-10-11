using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestStore
{
    public class DemoTest
    {
        [Fact]
        public void DemoTestRun()
        {
            Assert.True(1 > 0);
        }
    }
}
