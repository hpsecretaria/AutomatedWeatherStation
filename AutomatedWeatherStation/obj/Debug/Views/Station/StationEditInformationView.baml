   M S B A M L   `   `   `  ����  AutomatedWeatherStationF    @AutomatedWeatherStation.Views.Station.StationEditInformationView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      T PMahApps.Metro, Version=1.4.3.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;controls,http://metro.mahapps.com/winfx/xaml/controls  5      :d2http://schemas.microsoft.com/expression/blend/2008  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5        � DataContext  5	      ��    Locator �� Source# [   �� Path$$ StationsModule.SelectedStation� �� NotifyOnValidationError .   �� ValidatesOnDataErrors .   �� UpdateSourceTrigger$ PropertyChanged=�.��5      � .+�5   
   �  � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $��* �6    .+�5      �� $��0,0,0,10q�6    �� Orientation$ 
Horizontal=�6+   .+�5      �� $��Center=�6   $��13(�5      $
��Bold"�5      $��Station Information��5      �� 5      $	��5,0q�6   $��Center=�5      
������ $
��SAVE��5      ��5      �� $ SaveEditCommand�5      �� 5       $��Center=�6   
������ $��CANCEL��5#      ��5"      �� $ CancelEditCommand�5#      5$      �� 5%      $��2��6   $	��300��5&      $
��Left=�5'      .+�5(      �� $��0,0,0,15q�6   $��Center=�6/   .+�5*      �� *  $MahApps.Metro.Controls.TextBoxHelper   ClearTextButton . 	   UseFloatingWatermark	 . 
   	Watermark$
 Complete Location��5,      $��14(�5-      $��SemiBold"�5.      ��5/      �� $ EditModel.Location� . $ PropertyChanged=�52      �� 53      $��0,0,0,15q�6   $��Center=�6/   .+�55      ��  . 	 . $
 City/Province��57      $��14(�58      $��SemiBold"�59      ��5:      �� $ EditModel.DisplayLocation� . $ PropertyChanged=�5=      �� 5>      $��0,0,0,15q�6   $��Center=�6/   .+�5?      ��  . 	 . $

 Area��5A      $��14(�5B      $��SemiBold"�5C      ��5D      �� $# EditModel.DisplayLocationArea� . $ PropertyChanged=�5G      �� 5H      $��0,0,0,15q�6   $��Center=�6/   .+�5J      ��  . 	 . $

 IMEI��5L      $��14(�5M      $��SemiBold"�5N      ��5O      �� $ EditModel.Imei� . $ PropertyChanged=�5R      �� 5S      $��0,0,0,15q�6   $��Center=�6/   .+�5U      ��  . 	 . $
 Phone Number (9*********)��5W      $��14(�5X      $��SemiBold"�5Y      ��5Z      �� $ EditModel.PhoneNumber� . $ PropertyChanged=�5]      �� 5^      $��0,0,0,15q�6   $��Center=�6/   .+�5_      ��  . 	 . $
 Latitude��5a      $��14(�5b      $��SemiBold"�5c      ��5d      �� $ EditModel.LatitudeString� . $ PropertyChanged=�5g      �� 5h      $��0,0,0,15q�6   $��Center=�6/   .+�5i      ��  . 	 . $
 	Longitude��5k      $��14(�5l      $��SemiBold"�5m      ��5n      �� $ EditModel.LongitudeString� . $ PropertyChanged=�5q      �� 5r      $��0,0,0,15q�6   $��Center=�6/   .+�5s      ��  . 	 . $
 Keyword��5u      $��14(�5v      $��SemiBold"�5w      ��5x      �� $ EditModel.Keyword� . $ PropertyChanged=�5{      5|      5}      5~      5      