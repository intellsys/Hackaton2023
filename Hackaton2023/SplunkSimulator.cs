using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton2023
{
    class SplunkSimulator
    {
        private static Random random = new Random();
        public static byte[] GetStraightTrainingSample()
        {
            var sample = new byte[SupportBot.SEQUENCE_LENGTH];
            for (int i = 0; i < SupportBot.SEQUENCE_LENGTH; i++)
            {
                sample[i] = (byte)random.Next(12, 32);
            }

            return sample;
        }

        public static byte[] GetSpikeTrainingSample()
        {
            var sample = new byte[SupportBot.SEQUENCE_LENGTH];
            int spike_start = ((7 * SupportBot.SEQUENCE_LENGTH) / 8) + random.Next(-2, 8);
            for (int i = 0; i < SupportBot.SEQUENCE_LENGTH; i++)
            {
                if (i > spike_start)
                {
                    int s = (sample[i - 1] + random.Next(-2, 16));
                    s = s < 0 ? 0 : s;
                    s = s > 255 ? 255 : s;
                    sample[i] = (byte)s;
                }
                else
                {
                    int s = random.Next(12, 32);                    
                    sample[i] = (byte)s;
                }
            }

            return sample;
        }

        public static byte[] GetCyclicTrainingSample()
        {
            var sample = new byte[SupportBot.SEQUENCE_LENGTH];
            sample[0] = (byte)random.Next(12, 32);
            for (int i = 1; i < SupportBot.SEQUENCE_LENGTH; i++)
            {
                if (i > (SupportBot.SEQUENCE_LENGTH / 2) + random.Next(-8, 8))
                {
                    int s = sample[i - 1] + random.Next(-2, 1);
                    s = s < 0 ? 0 : s;
                    s = s > 255 ? 255 : s;
                    sample[i] = (byte)s;
                }
                else
                {
                    int s = sample[i - 1] + random.Next(-1, 4);
                    s = s < 0 ? 0 : s;
                    s = s > 255 ? 255 : s;
                    sample[i] = (byte)s;
                }
            }

            return sample;
        }
    }
}
