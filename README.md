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
• Making 1D arrays with size [256*256*256] <br>
• Indexing the array with following equation which make sure that that color's unique, ((red *256)+green)*256 + blue <br>
• Store the positions of distinct colors in the storeDcolors array for replacement <br>
• Space is required only 1D array with size [256*256*256] => which bounded by 𝐷^2 <br>
• Final Complexity is O( Height * Width) => O(𝑁^2) <br>
