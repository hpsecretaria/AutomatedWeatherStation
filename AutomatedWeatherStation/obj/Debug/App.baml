   M S B A M L   `   `   `  ����R  NAutomatedWeatherStation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:AutomatedWeatherStation.ModulesAutomatedWeatherStation.Modules  5      !    AutomatedWeatherStation.App   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      T PMahApps.Metro, Version=1.4.3.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;controls,http://metro.mahapps.com/winfx/xaml/controls  5      ;modules-clr-namespace:AutomatedWeatherStation.Modules  5        �� 	Resources  5      6   0AutomatedWeatherStation.Modules.ViewModelLocator   Locator  LabelTitleStyle �� Property �� Value  LabelSubTitleStyle  
LabelStyle  TextBlockStyle  ButtonNavigatorStyle  ListBoxStyle  
MenuLevel3  MetroTabItem �� BasedOn �� 
TargetType+  %MahApps.Metro.Controls.ControlsHelper 	 TabControlDisableStyle �� Triggers P� Property P� Value 
 MetroListBox  
WhiteBrush  
BlackBrush �� 
TargetType	 J� SnapsToDevicePixels
 �� Triggers  
GrayBrush9  
MenuLevel2 ��Name � ColumnDefinitions ��Name � RowDefinitions ��Name-  'MahApps.Metro.Controls.ContentControlEx  Name �� HeaderStringFormat �� ContentStringFormat   HeaderFontSize   HeaderFontStretch   HeaderFontWeight   RecognizesAccessKey   ContentCharacterCasing �� RelativeSource �� Path ��Name �� Mode �� AncestorType   BooleanToVisibilityConverter �� 	Converter �� 
TargetName  AccentColorBrush4  GrayNormalBrush P� 
SourceName  ]� 
Conditions! �� 
SourceName" �� Property# �� Value  HighlightColor  AccentColor  AccentColor2  AccentColor3  AccentColor4  HighlightBrush$ �� Color  AccentColorBrush  AccentColorBrush2  AccentColorBrush3  WindowTitleColorBrush  ProgressBrush% �� 
StartPoint& �� EndPoint' � Offset( � Color  CheckmarkFill  RightArrowFill  IdealForegroundColor   IdealForegroundColorBrush  ! IdealForegroundDisabledBrush) �� Opacity " AccentSelectedColorBrush  # MetroDataGrid.HighlightBrush $$  MetroDataGrid.HighlightTextBrush )% %MetroDataGrid.MouseOverHighlightBrush "& MetroDataGrid.FocusBorderBrush 1' -MetroDataGrid.InactiveSelectionHighlightBrush 5( 1MetroDataGrid.InactiveSelectionHighlightTextBrush* �� MergedDictionaries+ �� Source�� 5   
   * 5     �� 5     $+ Converter/Converters.xaml?�6%   �� 5     $+ /Resources/Icons.xaml?�6%   �� 5     $I+ Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   �� 5     $F+ @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5     $G+ Apack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml?�6%   �� 5#     $R+ Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?�5$     �� 5%     $\+ Vpack://application:,,,/MahApps.Metro;component/Styles/Controls.AnimatedTabControl.xaml?�5&     �� 5(     $!+ Template/DataTemplates.xaml?�6%   %�  &	        &	      &	 �     &	 �    &	 Y    &	 @    &	     &	     7  &		 �    7  &	
 �    &	 e    7  7  &	 �    &	 �    &	     &	 4    &	 S    &	 r    7  &	 �    7  &	 �    7  &	 �    7  &	 �    7  &	 �    7  &	     7  7  &	 �    7  &	 �    7  &	 �    &	  �    7  &	! �    7  &	" $    7  &	# <    7  &	$ T    7  &	% l    7  &	& �    7  &	' �    7  &	( �    7    5	      6'   �� 5
      �5      �� 5       � ��$
 Left=�6B   �� 5       � ��$ Center=�6@   �� 5       � ��$ DemiBold"�69   �� 5       � ��
 �����69   5      �� 5      �5      �� 5       � ��$
 Left=�6B   �� 5       � ��$ Center=�6@   �� 5       � ��$ DemiBold"�69   �� 5       � ��
 �   �69   5      �� 5      �5      �� 5       � ��$
 Left=�6B   �� 5       � ��$ Center=�6@   �� 5       � ��$ SemiBold"�69   �� 5       � ��
 �   �69   5!      �� 5#      �5$      �� 5%       � ��$ 5,0,0,0q�69   �� 5&       � ��$
 Left=�6F   �� 5'       � ��$ Center=�6D   �� 5(       � ��
 �   �6=   �� 5)       � ��$
 Wrap=�6?   5+      �� 5.      �5/      �� 50       � ��$ Stretch=�6C   �� 51       � ��
 ���� 6:   �� 52       � ��
 ���� 6;   �� 53       � ��$ 5,0,5,0q�66   55      �� 56      �57      �� 58       � ��$ Stretch=�6D   �� 59       � ��$ Stretch=�6B   �� 5:       � ��
 ���� 6;   �� 5;       � ��
 ���� 6<   �� 5<       � ��
 �   �6;   �� 5=       � ��$ 5q�67   5?      �� 5A      8   " ��5C      .�5D      ��  �@ HeaderFontSize$ 19Q�6K   �� 5E       �@ HeaderFontWeight$ Medium"�6M   5H      �� 5K      8   " ��5M       5N      P� 5O       �@��
IsSelected .  . �5P      ��  � �� �#D4D1D167   �� 5Q       � �� �#D4D1D16C   5R      P� 5S       �@��
IsSelected . . �5T      ��  � ��
 �����67   �� 5U       � ��
 �����6C   5V      5X      �� 5Z      " ��6)   .�5[      ��  � ��# �  6/   �� 5\       � ��# �  60   �� 5^       � ��$ 0q�64   �� 5_       � ��$ Stretch=�6?   �� 5`       � ��$ Center=�6=   �� 5a       � �� . 6B   �� 5b       � ��$
 Auto=�6O   �� 5c       �@��PanningMode$
 Both=�6=   �� 5d       � ��$
 Auto=�6M   �� 5e       �@J�SnapsToDevicePixels . 68   �� 5f       �@��IsFlicksEnabled .  6;   �� 5g       � �� 5h      �� 5i      " ��6*   .R�5j      �� $��Border��6%   #��z��5k   %   #��z��5l   %   #��z��5m   %   ._�5n   "   �� #��z��6/   |�.  #��z��5p   /   #��z��5q   /   #	 z	 5r   /   #��z��5s   /   .��5t   &   �� #	 z	 65   5u   #   5v      
 5w      P� 5x   "    �@��
IsGrouping .  . �5y   &   ��  � �� .  6V   5z   #   5|      5~       5      P� 5�       � {� .  . �5�      ��  � ��# �  68   5�      P� 5�       �@%�IsVirtualizing . . �5�      ��  � �� 5�      �� 5�   "   .R�5�   &   $� 5�   #   5�      �� 5�       � �� . 6J   5�      5�      �� 5�      8   " ��5�      .�5�      ��  �@ HeaderFontSize$ 22Q�6K   �� 5�       �@ HeaderFontWeight$ Medium"�6M   �� 5�       � �� 5�      �� 5�      " ��6*   .R�5�      �� $ Border��6%   $��Stretch=�5�   %   #��z��5�   %   #��z��5�   %   #��z��5�   %   #	 z	 5�   %   ._�5�   "   � $��Stretch=�6'   $��Stretch=�6E    5�   &   �� 5�   *   $ PART_ContentLeftCol��6;   $
��Auto �6X   �� 5�   *   $ PART_ContentRightCol��6;   $
��Auto �6Y    5�   &   �� 5�   *   $ PART_ContentTopRow��68   $
��Auto �6T   �� 5�   *   $ PART_ContentBottomRow��68   $
��Auto �6W   .+�5�   &     $ ContentSite��6@   $��0��5�   @   $��0��5�   @   #��z��5�   @   # z 5�   @   #��z��5�   @   #��z��5�   @   #��z��5�   @   #��z 5�   @   #��z 5�   @   #��z��5�   @   #��z 5�   @   #��z��5�   @   #��z��5�   @    . #	 z	 5�   @    5�   @   ��  �� *TemplatedParent+$6 0(controls:ControlsHelper.ContentCharacterCasing)�5�   @   �� 5�   &   $ 	Underline��60   $��1��5�   0   $��0��5�   0   $
��Auto��5�   0   $��2��5�   0   $��0 1 0 0q�5�   0   $��Stretch=�5�   0   y�5�   0   ��  �� $ FindAncestor=�" ��$. ((controls:TabControlHelper.IsUnderlined)�8  5�   #   5�      
 5�      P� 5�   "    �@��
IsSelected . . �5�   &   �� $ ContentSite��6-    � ��
 �����6h   �� 5�   &   $ 	Underline��6-   # �  5�   -    � ��5�   #   P� 5�   "    �@��
IsSelected .  . �5�   &   �� $ ContentSite��6-   # �  5�   -    � ���� 5�   &   $ 	Underline��6-   # �  5�   -    � ��5�   #   P� 5�   "   $ ContentSite��6*    �@J�IsMouseOver . . �5�   &   �� $ ContentSite��6-   # �  5�   -    � ���� 5�   &   $ 	Underline��6-   # �  5�   -    � ��5�   #   ]� 5�   "     5�   &   �� 5�   *   $! ContentSite��64   " �@J�IsMouseOver# . 6d   �� 5�   *   " �@��
IsSelected# . 6J   ./�5�   &   �� $ ContentSite��6-    � ��
 �����6h   �� 5�   &   $ 	Underline��6-    � ��
 �����6T   5�   #   5�      5�      5�      ��5�      #3E3E3E��64   ��5�      #000000��61   ��5�      #858383��62   ��5�      #D4D1D1��62   ��5�      #EEEEEE��62   �� 5�      8$   65   �� 5�      8$   67   �� 5�      8$   68   �� 5�      8$   68   �� 5�      8$   68   �� 5�      8$   6<   �� 5�      $% 	1.002,0.5&�68   $& 	0.001,0.5&�6O   .��5�      � $' 0Q�6   8(   6*   � 5�      $' 1Q�6   8(  6*   5�      �� 5�      8$   64   �� 5      8$   65   ��5     	White��68   �� 5     8$   6@   �� 5     $	) 0.4Q�5     8$   5     �� 5     8$   6?   �� 5	     8$   6C   �� 5
     8$   6G   �� 5     8$   6L   �� 5     8$   6E   �� 5     8$   5     �� 5     8$   5     5.     51     