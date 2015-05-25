using System;

namespace InventoryQuest
{
    [Serializable]
    public class ImageIDPair
    {
        private int _ImageIDItem;
        private int _ImageIDType;

        public ImageIDPair()
        {
        }

        public ImageIDPair(int type, int item)
        {
            ImageIDType = type;
            ImageIDItem = item;
        }

        public int ImageIDType
        {
            get { return _ImageIDType; }
            set { _ImageIDType = value; }
        }

        public int ImageIDItem
        {
            get { return _ImageIDItem; }
            set { _ImageIDItem = value; }
        }

        public override string ToString()
        {
            return "(" + ImageIDType + ", " + ImageIDItem + ")";
        }
    }
}