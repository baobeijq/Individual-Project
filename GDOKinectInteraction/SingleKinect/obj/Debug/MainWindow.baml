   M S B A M L   `   `   `  ����G  CSingleKinect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:SingleKinectSingleKinect  5   	       SingleKinect.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6k(http://schemas.microsoft.com/kinect/2014     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:SingleKinect     5   	   =controls(http://schemas.microsoft.com/kinect/2014     5   	     � Title$  
MainWindow��5
   	   $	��456��6   $��1548.06��6)   .��5      � .+�5   
   �� -   5         ��Name$ bodyCanvas_Border_��5      $��Right=�6-   $	��512��6I   
���   �$��2q�6i   $��0,1.834,514,0.834q�6}   ._�5      �� -   5         ��Name$ allPeopleLable��5      $��
All People��6,   $��Center=�6A   $	��Top=�5      $��16(�6    $
��Bold"�6.   $��199,-2,217,0q�6@   5      �� 5   
   -   5         ��Name$ 	bodyImage��5      $	��510��6#   $��Right=�6/   $��0,3,514.393,-0.333q�6K   �� 5   
   -   5         ��Name$ 
bodyCanvas��5      $	��512��6%   $	��424��61   $��Center=�6>   �� 5   
   $
��Left=�6   .+�5      �� -   5        $ 	leftLabel��5      $��HandLeftState: ��6'   $��Center=�6A   �� 5      -   5        $ 
rightLabel��5      $��HandRightState: ��6(   $��Center=�6C   �� 5      -   5        $ 	faceLabel��5      $��Face Yaw Pitch Roll: ��6'   $��Center=�6G   5      �� 5   
   -   5        $ engager2_Border_��5      $
��Left=�6+   $	��512��6F   
���   �$��2q�6f   $��0,2,0,-1.333q�6z   ._�5      �� -	   5        $ engagerLabel��5      $��Current Engager��6*   $��Center=�6D   $	��Top=�5      $��16(�6#   $
��Bold"�61   5      �� 5   
   -
   5        $ engagerImage��5      $	��512��6&   $
��Left=�62   $��0,1,0,1.667q�6M   �� 5   
   -   5        $ engagerCanvas��5      $	��424��6(   $��0,1,1029.333,1.667q�65   �� 5   
   -   5        $ transform_Border_��5      $��Right=�6,   $	��514��6H   
���   �$��3q�6h   $��0,1,3.373,0.667q�6|   ._�5       �� -   5        $ transformLabel1��5       $��	Transform��6-   $��Center=�6A   $��16(�6^   $
��Bold"�6l   $��199.333,0,199.667,378.333q�6~   $	��109��6�   5!      �� 5"   
   -   5        $ transformImage��5"      $	��514��6(   $��Right=�64   $��0,3,0.393,-0.333q�6P   �� 5#   
   -   5        $ transformCanvas��5#      $��Right=�6*   $	��425��6F   $��
1027,0,0,0q�6S   $��Center=�6g   $	��514��6�   5$      5&      