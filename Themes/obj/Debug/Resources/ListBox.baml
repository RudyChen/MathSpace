   M S B A M L   `   `   `  ����\  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius   ListBox.Background �� Color  BorderMediumColor K� IsItemsHost	 �� Triggers
 P� Property P� Value ��Name �� Padding J� SnapsToDevicePixels'    !System.Windows.VisualStateManager    VisualStateGroups%   System.Windows.VisualStateGroup   States    System.Windows.VisualState   
Storyboard �� 
TargetName �� TargetProperty8  2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime # ListBoxItem.MoveOver.Background �� Value ' #ListBox.Selected.Focused.BackGorund ) %ListBox.Selected.UnFocused.BackGorund�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml      5        5      �� 5   
   $ Shared.xaml?�6   %�  '��       7  '�� -    �� 5	      " ��5
   
   .�5   
   ��  �@J�SnapsToDevicePixels . 5      �� 5   
    �@�OverridesDefaultStyle . 5      �� 5   
    � ��$
 Auto=�5      �� 5   
    � ��$
 Auto=�5      �� 5   
    � �� . 5      �� 5   
    � ��$	 120��5      �� 5   
    � ��$ 95��5      �� 5   
    � �� 5      �� 5      " ��6"   .R�5      �� $��Border��6   $��0q�5      $ 2��5      ��5      �� 5       # �   6.   ��5"      �� 5#      8   6.   ._�5%      �� $��0q�6'   |�.  .��5&      ��  . ��5'   "   �� 5(   &   # �   66   5*      5+      5,      	 5-      P� 5.      
 �@��
IsGrouping . . �50      ��  � �� .  51      52      54      56      57      �� 59      " ��5:   
   .�5;   
   ��  �@J�SnapsToDevicePixels . 5<      �� 5=   
    �@�OverridesDefaultStyle . 5>      �� 5?   
    � �� 5@      �� 5A      " ��6"   .R�5B      �� $ Border��6   $ 2q�5C       . $��8,1,0,1q�5D   .   ��5E      �� 5F      $ Transparent��6.    5H        5I      CommonStates��. 5J   "     
Normal��6.     5K   "   	MouseOver��. 5L   &   �� .��5M   *   �� $ Border��6G   $0 *(Panel.Background).(SolidColorBrush.Color)�5N   2   .g�5O   .     $ 0��6B   # �  5P   ,   5Q   +   5R   '   5S   #   5T        5U      SelectionStates��. 5V   "     
Unselected��6.     5W   "   Selected��. 5X   &   �� .��5Y   *   �� $ Border��6G   $0 *(Panel.Background).(SolidColorBrush.Color)�5Z   3   .g�5[   .     $ 0��6B   # �  5\   ,   5]   +   5^   '   5_   #     5`   "   SelectedUnfocused��. 5a   &   �� .��5b   *   �� $ Border��6G   $0 *(Panel.Background).(SolidColorBrush.Color)�5c   2   .g�5d   .     $ 0��6B   # �  5e   ,   5f   +   5g   '   5h   #   5i      ._�5k      �� 5l      5m      5o      5p      5r      