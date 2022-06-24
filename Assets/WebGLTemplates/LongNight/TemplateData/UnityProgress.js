function UnityProgress(unityInstance, progress)
{
  if (!unityInstance.Module)
    return;

  if (!unityInstance.logo)
  {
    unityInstance.logo = document.getElementById("custom-logo");
    unityInstance.logo.style.display = "block";
    unityInstance.container.appendChild(unityInstance.logo);
  }

  if (!unityInstance.progress)
  {
    unityInstance.progress = document.getElementById("custom-loader");
    unityInstance.progress.style.display = "block";
    unityInstance.container.appendChild(unityInstance.progress);
  }

  setLoaderProgressTo(progress);

  if (progress == 1)
  {
    unityInstance.logo.style.display = "none";
    unityInstance.progress.style.display = "none";
  }
}

// value - 0 to 1
function setLoaderProgressTo(value)
{
  const fillText = unityInstance.progress.getElementsByClassName("label")[0];
  fillText.textContent = (value * 100).toFixed() + "%";
}
