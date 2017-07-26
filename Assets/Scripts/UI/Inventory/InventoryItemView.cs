using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour, IPointerClickHandler {

	[SerializeField]
	private Image icon;

	private Item item;

    public void SetItem( Item item ) {

		this.item = item;
		icon.sprite = item.info.inventoryIcon;
	}

	public void OnPointerClick( PointerEventData eventData ) {

		item.Apply();
	}
}
