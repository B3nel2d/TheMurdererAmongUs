//====================================================================================================
//
//  EventAudioPlayer
//
//  UIパーツの音声再生イベントを規定するスクリプト
//
//====================================================================================================

using UnityEngine;
using UnityEngine.EventSystems;

public class EventAudioPlayer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler{

    //各音声
    [SerializeField] private AudioClip audioOnPointerEnter;
    [SerializeField] private AudioClip audioOnPointerClick;

    /// <summary>
    /// ポインターがUIパーツ上に入った際のイベント
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData){
        if(audioOnPointerEnter != null){
            UIManager.instance.PlayAudio(audioOnPointerEnter);
        }
    }

    /// <summary>
    /// UIパーツがクリックされた際のイベント
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData){
        if(audioOnPointerClick != null){
            UIManager.instance.PlayAudio(audioOnPointerClick);
        }
    }

}
