   M S B A M L   `   `   `  ����  AutomatedWeatherStationB    <AutomatedWeatherStation.Views.Station.StationInformationView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        � DataContext  5      ��    Locator �� Source# [   �� Path$$ StationsModule.SelectedStation�.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0,0,0,10q�6    �� Orientation$ 
Horizontal=�6'   .+�5      �� $��Center=�6   $��13(�5      $
��Bold"�5      $��Station Information��5      �� 5      $	��5,0q�6   $��Center=�5      
������ $
��EDIT��5      ��5      �� $ EditCommand�5      5      �� 5   
   $��1��6   .+�5      �� $��0,0,0,15q�6   .+�5      �� $��Center=�6   $��11(�5      $
��Bold"�5       $��Complete Location��5!      �� 5"      $��Center=�6   $��13(�5#      ��5$      �� $ Model.Location�5%      �� 5&      $��0,0,0,15q�6   .+�5'      �� $��Center=�6   $��11(�5(      $
��Bold"�5)      $��City/Province��5*      �� 5+      $��Center=�6   $��13(�5,      ��5-      �� $ Model.DisplayLocation�5.      �� 5/      $��0,0,0,15q�6   .+�50      �� $��Center=�6   $��11(�51      $
��Bold"�52      $
��Area��53      �� 54      $��Center=�6   $��13(�55      ��56      �� $ Model.DisplayLocationArea�57      �� 58      $��0,0,0,15q�6   .+�59      �� $��Center=�6   $��11(�5:      $
��Bold"�5;      $
��IMEI��5<      �� 5=      $��Center=�6   $��13(�5>      ��5?      �� $ 
Model.Imei�5@      �� 5A      $��0,0,0,15q�6   .+�5B      �� $��Center=�6   $��11(�5C      $
��Bold"�5D      $��Phone Number��5E      �� 5F      $��Center=�6   $��13(�5G      ��5H      �� $ Model.PhoneNumber�5I      �� 5J      $��0,0,0,15q�6   .+�5K      �� $��Center=�6   $��11(�5L      $
��Bold"�5M      $��Latitude��5N      �� 5O      $��Center=�6   $��13(�5P      ��5Q      �� $ Model.Latitude�5R      �� 5S      $��0,0,0,15q�6   .+�5T      �� $��Center=�6   $��11(�5U      $
��Bold"�5V      $��	Longitude��5W      �� 5X      $��Center=�6   $��13(�5Y      ��5Z      �� $ Model.Longitude�5[      �� 5\      $��0,0,0,15q�6   .+�5]      �� $��Center=�6   $��11(�5^      $
��Bold"�5_      $��Keyword��5`      �� 5a      $��Center=�6   $��13(�5b      ��5c      �� $ Model.Keyword�5d      5f      5g      5h      