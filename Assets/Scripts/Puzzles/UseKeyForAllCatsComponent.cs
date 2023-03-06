using UnityEngine;

public class UseKeyForAllCatsComponent : MonoBehaviour
{
   public void UseKeyForAllCats()
   {
      GameObject[] cats = GameObject.FindGameObjectsWithTag("Cat");
      foreach (var cat in cats)
      {
         var player = cat.GetComponent<Player>();
         if (player != null)
         {
            player.UseKey();
         }
      }
   }
}
