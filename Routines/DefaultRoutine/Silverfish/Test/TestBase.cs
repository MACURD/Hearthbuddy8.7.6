using System.IO;
using HREngine.Bots;

namespace HREngine.Bots
{
    public class TestBase
    {
        public void InitSetting()
        {
            Settings.Instance.writeToSingleFile = true;
            if (Hrtprozis.Instance.settings == null)
            {
                Hrtprozis.Instance.setInstances();
                ComboBreaker.Instance.setInstances();
                PenalityManager.Instance.setInstances();
            }

        }
    }
}
