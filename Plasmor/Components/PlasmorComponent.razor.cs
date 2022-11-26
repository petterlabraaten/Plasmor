using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace Plasmor.Components
{
    public partial class PlasmorComponent
    {
        const int Width = 320;
        const int Height = 240;

        protected override async Task OnInitializedAsync()
        {
            if (OperatingSystem.IsBrowser())
            {
                await JSHost.ImportAsync("PlasmorComponent", "../Components/PlasmorComponent.razor.js");
                await Interop.OnInit(this);
            }

            DoRender();
        }

        byte[] buffer = new byte[Width * Height * 4];

        int j = 0, k = 0, l = 0;
        int f = 1000;

        protected void DoRender()
        {
            j = (j + 13) % f;
            k = (k + 17) % f;
            l = (l + 19) % f;

            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                {
                    var r =
                        (
                        +(128 + Math.Sin(2 * Math.PI * (j + 2 * x + 2 * y) / f) * 128)
                        + (128 + Math.Sin(2 * Math.PI * (-k + y) / f) * 128)
                        ) / 2;

                    var g =
                        (
                        +(128 + Math.Sin(2 * Math.PI * (k + x + y) / f) * 128)
                        + (128 + Math.Sin(2 * Math.PI * (-j + x) / f) * 128)
                        ) / 2;

                    var b =
                        (
                        +(128 + Math.Sin(2 * Math.PI * (l + x - y) / f) * 128)
                        + (128 + Math.Sin(2 * Math.PI * (-l - k - x + y) / f) * 128)
                        ) / 2;

                    var p = (x + Width * y) * 4;

                    buffer[p + 0] = (byte) r;
                    buffer[p + 1] = (byte) g;
                    buffer[p + 2] = (byte) b;
                    buffer[p + 3] = 255;
                }

            Interop.DoStuff(buffer);

            Interop.RequestAnimationFrame(DoRender);
        }



        [SupportedOSPlatform("browser")]
        public partial class Interop
        {
            [JSImport("onInit", "PlasmorComponent")]
            internal static partial Task OnInit([JSMarshalAs<JSType.Any>] object component);

            [JSImport("doStuff", "PlasmorComponent")]
            //internal static partial void DoStuff([JSMarshalAs<JSType.Array<JSType.Number>>] byte[] bytes);
            internal static partial void DoStuff([JSMarshalAs<JSType.MemoryView>] Span<byte> bytes);

            [JSImport("requestAnimationFrame", "PlasmorComponent")]
            internal static partial void RequestAnimationFrame([JSMarshalAsAttribute<JSType.Function>] Action action);
        }
    }
}
