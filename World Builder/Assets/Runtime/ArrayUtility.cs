namespace WorldBuilder
{
    public static class ArrayUtility
    {
        public static int Flatten(int x, int y, int width)
        {
            return x + y * width;
        }

        public static int Flatten(int x, int y, int z, int width, int height)
        {
            return x + y * width + z * width * height;
        }

        public static (int x, int y) Reshape(int i, int width)
        {
            int x = i % width ;
            int y = i / width ;

            return (x, y);
        }
        
        public static (int x, int y, int z) Reshape(int i,  int width, int height)
        {
            int index = i;
            
            int z = index / (width * height);
            index = index % (width * height); 
            int y = index / width;
            int x = index % width;
            
            return (x, y, z);
        }
    }
}