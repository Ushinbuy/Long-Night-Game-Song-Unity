using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class Link : MonoBehaviour
{
	[DllImport("__Internal")]
	private static extern void openWindow(string url);

	[DllImport("__Internal")]
	private static extern void usualyClickFunction();

	private void OpenLink(string currentLink)
    {
#if !UNITY_EDITOR
		openWindow(currentLink);
#else
		Application.OpenURL(currentLink);
#endif
	}

	public void StartAgain()
	{
#if !UNITY_EDITOR
		usualyClickFunction();
#endif
		SceneManager.LoadScene(0);
	}

	public void OpenVK()
	{
		OpenLink("https://vk.com/vinlisa");
	}

	public void OpenYandex()
	{
		OpenLink("https://music.yandex.ru/artist/4367363");
	}

	public void OpenTelegramm()
	{
		OpenLink("https://t.me/vinlisa");
	}

	public void OpenSpotify()
	{
		OpenLink("https://open.spotify.com/artist/0XlRDlIZEEvsdWjSnKtHDZ");
	}

	public void OpenYoutube()
	{
		OpenLink("https://music.youtube.com/channel/UCREd1CmI5v57KSfZQ7cNbfA");
	}

	public void OpenApple()
	{
		OpenLink("https://music.apple.com/ru/artist/%D0%B2%D0%B8%D0%BD%D0%BE%D0%B3%D1%80%D0%B0%D0%B4%D0%BD%D0%B0%D1%8F-%D0%BB%D0%B8%D1%81%D0%B0/1108820442");
	}
}