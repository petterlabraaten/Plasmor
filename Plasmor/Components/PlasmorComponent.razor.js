let bitmap = null;
let ctx = null;

export async function onInit(component) {
    var canvasElement = document.getElementsByClassName('canvas')[0];
    ctx = canvasElement.getContext("2d");
    bitmap = ctx.createImageData(320, 240);
}

export function doStuff(span) {

    bitmap.data.set(span.slice(0), 0);     // Works with Span<byte>
    //bitmap.data.set(span, 0);     // Works with Array<byte>
    ctx.putImageData(bitmap, 0, 0);
}

export function requestAnimationFrame(func) {
    window.requestAnimationFrame(func);
}