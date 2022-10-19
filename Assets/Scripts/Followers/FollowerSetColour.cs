using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Assets.Scripts.Followers
{
    
    public class FollowerSetColour : MonoBehaviour
    {
        [SF] private SkinnedMeshRenderer _character = default;
        [SF] private SkinnedMeshRenderer _bicycle = default;
        [SF] private FollowerColours _colours = null;

        public void Awake(){
            var shirts = _colours.Shirt;
            var shirt = shirts[Random.Range(0, shirts.Length)];
            _character.materials[0].color = shirt;

            var pantalons = _colours.Pants;
            var pants = pantalons[Random.Range(0, pantalons.Length)];
            _character.materials[1].color = pants;

            var skins = _colours.Skin;
            var skin = skins[Random.Range(0, skins.Length)];
            _character.materials[2].color = skin;

            var bikes = _colours.Bike;
            var bike = bikes[Random.Range(0, bikes.Length)];
            _bicycle.materials[1].color = bike;
        }
    }
}