function Show(can) {
       for(var i=0;i<4;i++){
           if(can==i){
               if(document.getElementById("div"+i).style.display=="none")
               {
                   document.getElementById("div"+i).style.display="block";
               }
               else
                   if(document.getElementById("div"+i).style.display=="block")
                   {
                       document.getElementById("div"+i).style.display="none";
                   }}
           else
               document.getElementById("div"+i).style.display="none";
       }
   }