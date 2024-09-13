using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtsEx.Extensions.SoundFactory;
using BveTypes.ClassWrappers;

namespace MetroDrive.MapPlugin
{
    internal class SoundControll
    {
        Sound sound;
        public void OnStart(ISoundFactory soundFactory,string Location)
        {
            string path = Path.Combine(Path.GetDirectoryName(Location), @"sound\pon.wav");
            sound = soundFactory.LoadFrom(path, 1, Sound.SoundPosition.Cab, 1);
        }
        public void PlaySound(ISoundFactory soundFactory ,string Location,string filename)
        {
            string path = Path.Combine(Path.GetDirectoryName(Location),@"sound\"+ filename);
            sound = soundFactory.LoadFrom(path, 1, Sound.SoundPosition.Cab,1);
            sound.Play(1, 1, 0);
        }
        public void OnDispose()
        {
            sound.Dispose();
        }
    }
}

