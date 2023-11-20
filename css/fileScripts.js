window.downloadFileFromStream = async (filename, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement("a");
    anchorElement.href = url;
    anchorElement.download = filename ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}
window.setImageUsingStreaming = async (imageElementId, imageStream) => {
    console.log("In set image")
    console.log("image element ID = " + imageElementId)
    const arrayBuffer = await imageStream.arrayBuffer();
    console.log("arraybuffer created")
    const blob = new Blob([arrayBuffer]);
    console.log("blob exectued")
    const url = URL.createObjectURL(blob);
    console.log("URL = " + url)

    const image = document.getElementById(imageElementId);

    console.log("After setting the image")
    if (image && image !== "null" && image !== "undefined") {
        console.log("Image is not null")
    } else {
        console.log("Image is null")
    }

    image.onload = () => {
        URL.revokeObjectURL(url);
    }
    console.log("image after being searched for = " + image.id)
    image.src = url;
}

window.test = async (test) => {
    console.log("In test")
}
