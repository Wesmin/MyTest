using UnityEngine;
delegate int TestC(int i);
class Delegate : MonoBehaviour
{
    void Start()
    {
        TestA(TestB);
        print("8");
    }
    void TestA(TestC intA)
    {
        print("2");
        print(intA(7));
        print("2");
    }
    int TestB(int intB)
    {
        print("3");
        return intB;
    }
}
