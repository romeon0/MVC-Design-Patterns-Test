using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.TestTools;
using UnityGame.MVC;
using UnityGame.Tests;

using Debug = UnityEngine.Debug;

public class UnitTestRunner
{
    // A Test behaves as an ordinary method
    [Test]
    public void SimpleTests()
    {
        //var testRunner = new UnityGame.Tests.TestRunner();
        //testRunner.Run();

        Debug.Log("Phase 1: AreEqual");
        Assert.AreEqual(1, 1);

        Debug.Log("Phase 2: IsTrue");
        Assert.IsTrue(IsAppRunning());

        Exception ex = Assert.Catch(MethodWithException);
        Debug.Log($"Phase 3, Ex:{ex.Message}");

        Debug.Log($"Phase 4");
        //Assert.True(false, "Hello message", "123");

        Debug.Log($"Phase 5");
        Assert.DoesNotThrow(MethodWithException);

        Debug.Log($"Phase 6");
        Assert.AreSame(23, 23);
    }

    [Test]
    public void EquipItemsTest()
    {
        InventoryController controller = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<InventoryController>();

        InventoryModel model = new InventoryModel()
        {
            items = new List<InventoryItem>()
            {
                new InventoryItem(){equipped = false, id = "id_1"},
                new InventoryItem(){equipped = true, id = "id_2"},
                new InventoryItem(){equipped = true, id = "id_3"},
                new InventoryItem(){equipped = false, id = "id_4"},
            }
        };
        controller.SetModel(model);

        controller.EquipItem("id_1");
        controller.UnequipItem("id_3");

        IList<InventoryItem> items = controller.GetItems();
        Assert.AreEqual(items[0].equipped, true);
        Assert.AreEqual(items[2].equipped, false);

        GameObject.DestroyImmediate(controller.gameObject);
    }

    private void MethodWithException()
    {
        Debug.Log("MethodWithException 1");
        throw new Exception("Exception occurred here, ouch!");
        Debug.Log("MethodWithException 2");
    }

    private bool IsAppRunning()
    {
        return true;// Application.isPlaying;
    }

    [UnityTest]
    public IEnumerator AssertIfNegative()
    {
        int timer = 4;
        while (timer >= -1)
        {
            Assert.Positive(timer);
            Debug.Log($"AssetIfLowerZero. Timer:{timer}");
            yield return null;
            --timer;
        }
    }
}
