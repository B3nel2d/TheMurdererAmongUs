using UnityEngine;
using UnityEngine.EventSystems;

public class EventAudioPlayer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler{

    [SerializeField] private AudioClip audioOnPointerEnter;
    [SerializeField] private AudioClip audioOnPointerClick;

    public void OnPointerEnter(PointerEventData eventData){
        if(audioOnPointerEnter != null){
            UIManager.instance.PlayAudio(audioOnPointerEnter);
        }
    }

    public void OnPointerClick(PointerEventData eventData){
        if(audioOnPointerClick != null){
            UIManager.instance.PlayAudio(audioOnPointerClick);
        }
    }

}
