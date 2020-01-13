using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using SpaceInvaders.Game_Managers;
using Unity;
using Unity.Lifetime;

namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UnityContainer Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            Container = new UnityContainer();

            // Register Types
            Container.RegisterType<IScoreManager, ScoreManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IHudManager, HudManager>(new ContainerControlledLifetimeManager());

            base.OnStartup(e);
        }

        /// <summary>
        /// Returns true if the current application has focus
        /// </summary>
        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
                return false;       

            var procId = Process.GetCurrentProcess().Id;
            GetWindowThreadProcessId(activatedHandle, out var activeProcId);

            return activeProcId == procId;
        }
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
    }
}