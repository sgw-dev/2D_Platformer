using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ShopSuite
    {

        [UnityTest]
        public IEnumerator ItemCanBePurchasedViaItemObject()
        {
            //Create new object to test
            ShopManager shop_manager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Shop")).GetComponent<ShopManager>();

            ShopManager.ShopItem shop_item = new ShopManager.ShopItem();
            shop_item.ID = 1;

            shop_manager.AddItemToShop(shop_item);

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(shop_manager.PurchaseItem(shop_item), true);

            //Get rid of the garbage
            Object.Destroy(shop_manager.gameObject);
        }

        [UnityTest]
        public IEnumerator ItemCanBePurchasedViaSlotId()
        {
            //Create new object to test
            ShopManager shop_manager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Shop")).GetComponent<ShopManager>();

            ShopManager.ShopItem shop_item = new ShopManager.ShopItem();
            shop_item.ID = 1;

            shop_manager.AddItemToShop(shop_item);

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(shop_manager.PurchaseItem(0), true);

            //Get rid of the garbage
            Object.Destroy(shop_manager.gameObject);
        }

        [UnityTest]
        public IEnumerator TwoItemsCanBeAddedToShop()
        {
            //Create new object to test
            ShopManager shop_manager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Shop")).GetComponent<ShopManager>();

            //Create test items
            ShopManager.ShopItem shop_item = new ShopManager.ShopItem();
            shop_item.ID = 1;
            ShopManager.ShopItem shop_item_two = new ShopManager.ShopItem();
            shop_item_two.ID = 2;

            //Add test items to list
            List<ShopManager.ShopItem> shop_items = new List<ShopManager.ShopItem>();
            shop_items.Add(shop_item);
            shop_items.Add(shop_item_two);

            //Run fuction with test parameter
            shop_manager.AddItemsToShop(shop_items);

            yield return new WaitForSeconds(0.1f);

            //Test result
            Assert.AreEqual(shop_manager.purchasable_items.Count, 2);

            //Get rid of the garbage
            Object.Destroy(shop_manager.gameObject);
        }

        [UnityTest]
        public IEnumerator MissingItemFromPurchaseBySlotReturnsFalse()
        {
            //Create new object to test
            ShopManager shop_manager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Shop")).GetComponent<ShopManager>();

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(shop_manager.PurchaseItem(0), false);

            //Get rid of the garbage
            Object.Destroy(shop_manager.gameObject);
        }

        [UnityTest]
        public IEnumerator MissingItemFromPurchaseByItemReturnsFalse()
        {
            //Create new object to test
            ShopManager shop_manager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Shop")).GetComponent<ShopManager>();

            ShopManager.ShopItem shop_item = new ShopManager.ShopItem();
            shop_item.ID = 1;

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(shop_manager.PurchaseItem(shop_item), false);

            //Get rid of the garbage
            Object.Destroy(shop_manager.gameObject);
        }

    }
}
