# Image-Quantization-T012
FCIS Algorithms Project 22'
# Structs , And Classes Implemented
## Edge Struct : 
• Used For Describe Edges in the graph <br>
• Key -> Parent Node <br>
• Value -> child Node <br>
• Weight -> cost on Edge <br>
## GlobalDataForProjectOperationsClass [static class]: 
• Used For the values that repeated a lot and needed in more than one place in code <br>
• Width -> Image Width <br>
• Height -> Image Height <br>
• distincitColorsCounter -> number of distinct colors in image <br>
• For MST [parent for each node array , costs array] <br>
• For Bonuses [number of predicted clusters , counter for predicted clusters , previous result of 
total standard deviation in the First Bonus] <br>
## ProjectOperationsClass: 
• Handle all functions that will be needed in the project <br>

# Construct the graph[1] :
## A - Find Distinct Colors <br>
• Making 1D arrays with size [256 * 256 * 256] <br>
• Indexing the array with following equation which make sure that that color's unique, ((red * 256)+green) * 256 + blue <br>
• Store the positions of distinct colors in the storeDcolors array for replacement <br>
• Space is required only 1D array with size [256 * 256 * 256] => which bounded by 𝐷^2 <br>
• Final Complexity is O( Height * Width) => O(𝑁^2) <br>

## B - Construct The Graph
• In this function we take only the edges that has minimum cost and store it to make the [Minimal Spanning Tree] => Using Prim's Algorithm <br>
• Looping on each Distinct color's children , take only the minimum edge , and the next iteration will start from this child • It costs O(D * D) => O(𝐷 ^ 2) <br>

# Find Minimal Spanning Tree[2] :
• Since while constructing the graph we take only the required edges that will be needed in the MST we only make one loop that loops on vertices to get the MST ,and its cost <br>
• Final Complexity is Bounded By => O(V) [Number of parents]<br>

# Extract the K clusters from the minimum spanning tree[3] :
• Define values for clustering Like <br>
• vistiedNodes Boolean array that mark each vertex that will be visited <br>
• redSum , greenSum , blueSum, counterForChilds , and currClusterIndex -> Used to calculate Average <br>
• adjList -> used after removing edges to add the nodes that are adjacent to each others <br>
• distincitColorsIndices -> Used to mark where is the distinct Color's cluster  <br>
• The our approach is to remove the maximum edges in the MST with (K – 1) Counter <br>
• After that we adding the nodes that adjacent to each other after removing the maximum edges <br>
• Final Complexity is Bounded By => O(K * D)[K : number of desired clusters , D: distinct Colors] <br>

# Find the representative color of each cluster[4] :
• Iterate on the distinct colors <br>
• If the vertex is not visited -> then make BFS on it to get the adjacent to this vertex <br>
• After that calculating the average of the adjacent colors in the same loop <br>
• Final Complexity is Bounded By => O(D)[ D: distinct Colors] <br>

# Quantize the image by replacing the colors of each cluster by its representative color [5] :
• Iterate on the distinct colors <br>
• Change the storeDcolors with it's new color [the average from cluster] <br>
• Then Iterate on Image and replace each distinct Color with the average color from cluster that updated in the storeDcolors Array <br>
• Final Complexity is O( Height * Width) => O(𝑁 ^ 2) <br>

# Bonuses
## First Bonus [Auto Detection For K clusters] :
• Calculate mean , and standard deviation <br>
• Remove Edges that lead to maximum standard deviation <br>
• Increase Counter of the detected clusters <br>
• If(the previous result of SD – current Result of SD <=0.0001) -> then return the number of predicted clusters <br>
• Else recursively repeat the previous steps after removing the high SD Edge <br>
• Recurrence T(N) = T(N – 1) + O(N) , T(Base) = 1 <br>
• Final Complexity is Bounded By => O(𝐸 ^ 2) [ E: number of Edges in MST] <br>

## Second Bonus [Determine Clusters in better way] :
• Calculate mean , and standard deviation <br>
• Remove Edges that has value bigger than SD + mean <br>
• Increase Q for each removement <br>
• If(Q <= K) -> remove high Edges and determine the average of the adjacent colors <br>
• Else determine new colors from the adjacent colors , construct new graph , calculating new MST <br>
• Recursion until Q = K <br>
• Final Complexity is Bounded By => O(𝐾 𝑥 𝐷 ^ 2)) [ K : number of clusters , E : number of Edges in MST] <br>

