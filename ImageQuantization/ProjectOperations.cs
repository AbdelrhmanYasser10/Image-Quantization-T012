using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ImageQuantization
{
    class ProjectOperations
    {
        public struct Edge
        {

            public int key;
            public int value;
            public double weight;
        }

        public static class GlobalDataForProjectOperationsClass {
            public static int width;
            public static int height;
            public static double mstCost;
            public static int distincitColorsCounter = 0;
            public static RGBPixel[] storeDcolors;

            //MST
            public static int[] parentOfEachNode;
            public static double[] costOfEdges;
            public static void setValues() {
                width = ImageOperations.GetWidth(MainForm.ImageMatrix);
                height = ImageOperations.GetHeight(MainForm.ImageMatrix);
                mstCost = 0.0;
                distincitColorsCounter = 0;
            }

            public static int numberOfPredictedClusters = 0;
            public static int counterOfPredictedClusters = 0;
            public static double prevResult = double.MaxValue;
            public static void resetKClusterValues() {
                numberOfPredictedClusters = 0;
                counterOfPredictedClusters = 0;
                prevResult = double.MaxValue;
            }
        }



        public static List<RGBPixel> getDistincitColorsInOriginalImage() //o( h * w) => O(N^2)
        {
            bool[] arrCheckIfColorExsist = new bool[256 * 256 * 256]; //O(1)
            GlobalDataForProjectOperationsClass.storeDcolors = new RGBPixel[256 * 256 * 256]; //O(1)
            List<RGBPixel> distincitColors = new List<RGBPixel>(); //O(1)

            //Total O(H*W)
            for (int i = 0; i < GlobalDataForProjectOperationsClass.height; i++) //O(H)
            {
                for (int j = 0; j < GlobalDataForProjectOperationsClass.width; j++) //O(W)
                {
                    int index = MainForm.ImageMatrix[i, j].red; //O(1)
                    index = (index * 256) + MainForm.ImageMatrix[i, j].green; //O(1)
                    index = (index * 256) + MainForm.ImageMatrix[i, j].blue; //O(1)
                    if (arrCheckIfColorExsist[index] == false)//O(1)
                    {
                        arrCheckIfColorExsist[index] = true; //O(1)
                        GlobalDataForProjectOperationsClass.storeDcolors[index] = MainForm.ImageMatrix[i, j]; //O(1)
                        distincitColors.Add(MainForm.ImageMatrix[i, j]);//O(1)
                    }
                }
            }

            GlobalDataForProjectOperationsClass.distincitColorsCounter = distincitColors.Count; //O(1)
            return distincitColors; //O(1)
        }

        public static double getDistance(RGBPixel firstColor, RGBPixel secondColor) { //O(1)

            double cost = (Math.Abs(firstColor.red - secondColor.red) * Math.Abs(firstColor.red - secondColor.red))
                            + Math.Abs((firstColor.green - secondColor.green) * Math.Abs(firstColor.green - secondColor.green))
                            + (Math.Abs(firstColor.blue - secondColor.blue) * Math.Abs(firstColor.blue - secondColor.blue));//O(1)
            cost = Math.Sqrt(cost); //O(1)
            return cost; //O(1)
        }


        //Take Only The Required Edges [which has minimum costs]
        public static void constructGraph(RGBPixel[] distincitColors) { //O(D^2)

            ///Prim's Algorithm [modification-Take Only The Edges that will be needed in the mst];
            GlobalDataForProjectOperationsClass.costOfEdges = Enumerable.Repeat(double.MaxValue, distincitColors.Length).ToArray(); //o(d)
            GlobalDataForProjectOperationsClass.parentOfEachNode = Enumerable.Repeat(-1, distincitColors.Length).ToArray(); //o(d)
            GlobalDataForProjectOperationsClass.costOfEdges[0] = 0; //Neglegate The First Node's Parent; //O(1)

            int nextnode = 0;//o(1)
            bool[] visited = new bool[distincitColors.Length]; //o(1)
            RGBPixel firstVertix; //o(1)
            RGBPixel secondVertix; //o(1)

            //construct graph [getting distances] [O(D^2)]
            for (int i = 0; i < distincitColors.Length - 1; i++)  // o(D)
            {
                int index = nextnode; //o(1)
                firstVertix = distincitColors[index]; //o(1)
                visited[index] = true; //o(1)
                double minimumCost = double.MaxValue; //o(1)
                for (int j = 0; j < distincitColors.Length; j++) // o(D)
                {
                    if (!visited[j]) //O(1)
                    {
                        secondVertix = distincitColors[j]; //O(1)

                        double cost = getDistance(firstVertix, secondVertix); //O(1)

                        if (cost < GlobalDataForProjectOperationsClass.costOfEdges[j]) //O(1)
                        {
                            GlobalDataForProjectOperationsClass.parentOfEachNode[j] = index; //O(1)
                            GlobalDataForProjectOperationsClass.costOfEdges[j] = cost; //O(1)
                        }

                        if (GlobalDataForProjectOperationsClass.costOfEdges[j] < minimumCost)//O(1)
                        {
                            minimumCost = GlobalDataForProjectOperationsClass.costOfEdges[j]; //O(1)
                            nextnode = j;//O(1)
                        }
                    }
                }
            }

        }

        public static Edge[] calculateMST(Edge[] mst, int dColorsCount) //O(V)
        {
            #region[Kruskal Algorithm - Doesn't Work!!]
            ///Kruskal Algorithm;
            //Dictionary<RGBPixel, int> allIndeciesThatWeNeed = new Dictionary<RGBPixel, int>();
            //List<Edge> mst = new List<Edge>();
            //int e = 0;

            //subset[] subsets = new subset[distincitColors.Count];
            //for (int i = 0; i < distincitColors.Count; i++) //o(v)
            //{
            //    subsets[i] = new subset();
            //    allIndeciesThatWeNeed.Add(distincitColors[i], i);
            //    subsets[i].parent = i;
            //    subsets[i].rank = 0;
            //}

            ////int index = 0;
            //while (e < distincitColors.Count - 1) //o(v)
            //{

            //    Edge next_edge = allEdges.Dequeue();

            //    int x = find(subsets, allIndeciesThatWeNeed[next_edge.key]);
            //    int y = find(subsets, allIndeciesThatWeNeed[next_edge.value]);

            //    if (x != y)
            //    {
            //        mst.Add(next_edge);
            //        Union(subsets, x, y);
            //        e++;
            //    }
            //}
            #endregion

            ///Just Looping on the Parent of vertices which costs only O(V) ==> That while we construct the graph
            ///we only take the required edges for mst
            ///
            mst = new Edge[dColorsCount - 1];
            for (int i = 1; i < GlobalDataForProjectOperationsClass.parentOfEachNode.Length; i++) //#iterations * loop bodyO(1) = o(V)
            {
                Edge e = new Edge();//o(1)
                e.key = GlobalDataForProjectOperationsClass.parentOfEachNode[i]; //o(1) //parent
                e.value = i; //o(1) //destenation
                e.weight = GlobalDataForProjectOperationsClass.costOfEdges[i]; //o(1)
                mst[i - 1] = e; //o(1)
                GlobalDataForProjectOperationsClass.mstCost += GlobalDataForProjectOperationsClass.costOfEdges[i]; //o(1)
            }

            return mst; //o(1)

        }


        /*Values For Clustering*/

        public static bool[] vistiedNodes;
        public static double redSum;
        public static double greenSum;
        public static double blueSum;
        public static int counterForChilds;
        public static List<int>[] adjList;
        public static int currClusterIndex;
        public static int[] distincitColorsIndcies;
        public static void ExtractKClustersFromMST(Edge[] mst, RGBPixel[] distincitColors, int K) //O(K * D)
        {
            distincitColorsIndcies = new int[distincitColors.Length]; //o(1)
            vistiedNodes = new bool[distincitColors.Length]; //o(1)
            adjList = new List<int>[distincitColors.Length]; //o(1)
            currClusterIndex = 0; //o(1)
            counterForChilds = 0; //o(1)
            int currentClusters = 0; //o(1)
            for (int j = 0; j < distincitColors.Length; j++) { adjList[j] = new List<int>(); } //o(D)

            /// Remove High Weights from MST
            while (currentClusters < (K - 1)) //O(K * D)
            {
                double max = double.MinValue; //O(1)
                int index = -1; //O(1)
                for (int j = 0; j < mst.Length; j++) //O(D)
                {
                    if (mst[j].weight > max) //O(1)
                    {
                        index = j; //O(1)
                        max = mst[j].weight;//O(1)
                    }
                }
                mst[index].weight = -1; //O(1)
                mst[index].key = -1; //O(1)
                currentClusters++; //O(1)
            }
            for (int i = 0; i < mst.Length; i++)
            { //O(E) -> E = Number of Edges in the mst
                if (mst[i].weight != -1 && mst[i].key != -1)
                { //O(1)
                    adjList[mst[i].key].Add(mst[i].value); //O(1)
                    adjList[mst[i].value].Add(mst[i].key); //O(1)
                }
            }



        }


        public static void BFS(int node, RGBPixel[] distincitColors) //O(E + V)
        {
            Queue<int> q = new Queue<int>(); //O(1)
            q.Enqueue(node); //O(1)
            while (q.Count > 0) //-O(V) *Body  ->> O(V + E)
            {
                node = q.Dequeue();//O(1)
                vistiedNodes[node] = true;//O(1)
                distincitColorsIndcies[node] = currClusterIndex;//O(1)
                counterForChilds++;//O(1)
                redSum += distincitColors[node].red;//O(1)
                greenSum += distincitColors[node].green;//O(1)
                blueSum += distincitColors[node].blue;//O(1)

                for (int i = 0; i < adjList[node].Count; i++) //->O(E)
                {
                    if (!vistiedNodes[adjList[node][i]]) q.Enqueue(adjList[node][i]);//O(1)
                }

            }
        }


        public static RGBPixel[] calculatingAverage(RGBPixel[] clusters, RGBPixel[] distincitColors) {

            for (int j = 0; j < distincitColors.Length; j++) //#iterations = D * O(V + E) -> O(D)
            {
                if (!vistiedNodes[j])
                {
                    counterForChilds = 0; //O(1)
                    redSum = 0;//O(1)
                    greenSum = 0;//O(1)
                    blueSum = 0;//O(1)
                    BFS(j, distincitColors); //O(|E| + |D|)
                    redSum /= counterForChilds;//O(1)
                    greenSum /= counterForChilds;//O(1)
                    blueSum /= counterForChilds;//O(1)
                    RGBPixel tmp = new RGBPixel();//O(1)
                    tmp.red = (byte)redSum;//O(1)
                    tmp.green = (byte)greenSum;//O(1)
                    tmp.blue = (byte)blueSum;//O(1)
                    clusters[currClusterIndex++] = tmp;//O(1)
                }
            }
            return clusters; //O(1)
        }
        

        public static void replaceImage(RGBPixel[] clusters, RGBPixel[] distincitColors) //O(N^2) => O(H * W)
        {
            for (int i = 0; i < distincitColors.Length; i++) //O(D)
            {
                int index = distincitColors[i].red; //O(1)
                index = (index * 256) + distincitColors[i].green;//O(1)
                index = (index * 256) + distincitColors[i].blue;//O(1)
                GlobalDataForProjectOperationsClass.storeDcolors[index] = 
                    clusters[distincitColorsIndcies[i]];//O(1)
            }
            for (int i = 0; i < GlobalDataForProjectOperationsClass.height; i++) //O(N^2)
            {
                for (int j = 0; j < GlobalDataForProjectOperationsClass.width; j++)
                {
                    int index = MainForm.ImageMatrix[i, j].red;//O(1)
                    index = (index * 256) + MainForm.ImageMatrix[i, j].green;//O(1)
                    index = (index * 256) + MainForm.ImageMatrix[i, j].blue;//O(1)
                    MainForm.ImageMatrix[i, j] = GlobalDataForProjectOperationsClass.storeDcolors[index];//O(1)
                }
            }

        }

        /// <summary>
        /// bounses
        /// 1st bonus done
        /// 2nd bonus done
        /// </summary>
        /// <param name="mst"></param>
        /// <returns></returns>
        /// 

        ///TOTAL IS O(E^2) That e is the number of edges
        //First Bonus
        public static int autoDetectionForKClusters(Edge[] mst) {

            double SD = 0; //standardDiv for current iteration //O(1)
            double mean = 0; //mean //O(1)
            List<Edge> copyList = new List<Edge>(mst); //copy mst values //O(E)
            List<double> allSD = new List<double>(); //O(1)
            for (int i = 0; i < mst.Length; i++) //O(1)
                mean += mst[i].weight;
            mean /= mst.Length; //O(1)
            for (int i = 0; i < mst.Length; i++) { //O(E)
                double sum = Math.Abs(mean - mst[i].weight);
                sum *= sum;
                allSD.Add(Math.Sqrt(sum / (mst.Length - 1)));
                SD += sum;
            }
            double result = Math.Sqrt(SD / (mst.Length - 1)); //full result for current iteration //O(1)

            int index = allSD.IndexOf(allSD.Max()); //get index of max value//O(1)
            copyList.RemoveAt(index); //remove max value //O(E)
            GlobalDataForProjectOperationsClass.counterOfPredictedClusters++; //O(1)
            if ((Math.Abs(GlobalDataForProjectOperationsClass.prevResult - result) <= 0.0001) || copyList.Count == 0) //O(1)
            {
                GlobalDataForProjectOperationsClass.numberOfPredictedClusters = GlobalDataForProjectOperationsClass.counterOfPredictedClusters; //O(1)
                return GlobalDataForProjectOperationsClass.numberOfPredictedClusters;//O(1)
            }

            GlobalDataForProjectOperationsClass.prevResult = result; //O(1)
            return autoDetectionForKClusters(copyList.ToArray()); //recursion with recurrence = T(N - 1) + O(N) , T(BASE) = 1
            // TOTAL IS O(E^2) That e is the number of edges

        }


        //O(K * D^2)
        //Second Bonus 
        public static List<RGBPixel> determineKClustersInBetterWay(List<Edge> mstList, RGBPixel[] distincitColors, int K) {
            int Q = 1;//O(1)
            vistiedNodes = new bool[distincitColors.Length];//O(1)
            adjList = new List<int>[distincitColors.Length]; //O(1)
            distincitColorsIndcies = new int[distincitColors.Length]; //o(1)
            constructGraph(distincitColors); //O(V^2)
            mstList = calculateMST(mstList.ToArray() , distincitColors.Length).ToList(); //O(V)
            List<double> costs = new List<double>();//O(1)
            for (int i = 0; i < mstList.Count; i++) { //O(E)
                costs.Add(mstList[i].weight);
            }
            List<RGBPixel> clusters = new List<RGBPixel>();//O(1)
            currClusterIndex = 0; //o(1)
            counterForChilds = 0; //o(1)
            redSum = 0.0; //o(1)
            greenSum = 0.0; //o(1)
            blueSum = 0.0; //o(1)

            double mean = 0;//O(1)
            double SD = 0;//O(1)
            for (int i = 0; i < mstList.Count; i++) //O(E)
                mean += mstList[i].weight; //O(1) 
            mean /= mstList.Count; //O(1)
            for (int i = 0; i < mstList.Count; i++) { //O(E)
                double sum = Math.Abs(mean - mstList[i].weight); //O(1)
                sum *= sum;//O(1)
                SD += sum;//O(1)
            }
            for (int i = 0; i < distincitColors.Length; i++) //O(D)
            {
                adjList[i] = new List<int>(); //O(1)
            }

            for (int i = 0; i < mstList.Count; i++) { //O(E)
                adjList[mstList[i].key].Add(mstList[i].value); //O(1)
                adjList[mstList[i].value].Add(mstList[i].key);//O(1)
            }
            SD = Math.Sqrt(SD / (mstList.Count - 1)); //O(1)
            for (int i = 0; i < mstList.Count; i++) //O(E * D) -> o(D^2)
            {
                if (mstList[i].weight > (mean + SD)) //O(1)
                {
                    adjList[mstList[i].key].RemoveAt(adjList[mstList[i].key].IndexOf(mstList[i].value)); //O(D^2) -->Upper
                    adjList[mstList[i].value].RemoveAt(adjList[mstList[i].value].IndexOf(mstList[i].key)); //O(D^2) -->Upper
                    costs.RemoveAt(costs.IndexOf(mstList[i].weight)); //O(D^2) -->Upper
                    mstList.RemoveAt(i);//O(D^2) -->Upper
                    Q++; //o(1)
                }
            }

            if (Q <= K) //O(1)
            {
                while (Q < K) //O(K * D^2)
                {
                    if (costs.Count == 0) break;
                    int indexOfMax = costs.IndexOf(costs.Max()); //O(D)
                    adjList[mstList[indexOfMax].key].RemoveAt(adjList[mstList[indexOfMax].key].IndexOf(mstList[indexOfMax].value));//O(D^2)
                    adjList[mstList[indexOfMax].value].RemoveAt(adjList[mstList[indexOfMax].value].IndexOf(mstList[indexOfMax].key));//O(D^2)
                    costs.RemoveAt(indexOfMax);//O(D)
                    mstList.RemoveAt(indexOfMax);//O(D)
                    Q++; //O(1)
                }
                for (int j = 0; j < distincitColors.Length; j++) //#iterations = D * O(v + e) --> O(D)
                {
                    if (!vistiedNodes[j])
                    {
                        counterForChilds = 0;
                        redSum = 0;
                        greenSum = 0;
                        blueSum = 0;
                        BFS(j, distincitColors); //o(|E| + |D|)
                        redSum /= counterForChilds;
                        greenSum /= counterForChilds;
                        blueSum /= counterForChilds;
                        RGBPixel tmp = new RGBPixel();
                        tmp.red = (byte)redSum;
                        tmp.green = (byte)greenSum;
                        tmp.blue = (byte)blueSum;

                        clusters.Add(tmp);
                        currClusterIndex++;
                    }
                }
                return clusters; //O(1)
            }
            else {

                for (int j = 0; j < Q; j++) //#iterations = D * O(v + e) --> O(D)
                {
                    if (!vistiedNodes[j])
                    {
                        counterForChilds = 0;
                        redSum = 0;
                        greenSum = 0;
                        blueSum = 0;
                        BFS(j, distincitColors); //o(|E| + |D|)
                        redSum /= counterForChilds;
                        greenSum /= counterForChilds;
                        blueSum /= counterForChilds;
                        RGBPixel tmp = new RGBPixel();
                        tmp.red = (byte)redSum;
                        tmp.green = (byte)greenSum;
                        tmp.blue = (byte)blueSum;
                        clusters.Add(tmp);
                        currClusterIndex++;
                    }
                }
                for (int i = 0; i < distincitColors.Length; i++) //O(D)
                {
                    int index = distincitColors[i].red; //O(1)
                    index = (index * 256) + distincitColors[i].green;//O(1)
                    index = (index * 256) + distincitColors[i].blue;//O(1)
                    GlobalDataForProjectOperationsClass.storeDcolors[index] =
                        clusters[distincitColorsIndcies[i]];//O(1)
                    distincitColors[i] = clusters[distincitColorsIndcies[i]];
                }
                MainForm.distincitColors = clusters.ToArray(); //O(K)
                clusters = determineKClustersInBetterWay(mstList, MainForm.distincitColors, K); //recursion
            }
            return clusters; //O(1)
        }
    }
}
