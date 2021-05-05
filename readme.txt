# WBAssistantF

这是一个为学校白板写的辅助软件。虽然这个项目里面好像没有什么亮点，但我还是要把它放出来！

- 功能

1. U盘文件复制 (Module.USB)
    用了 System.Management(Nuget包) 中的 ManagementEventWatcher 监听U盘插入事件。

2. 每日壁纸切换 (Module.Wallpaper)
    鉴于学校白板不给网络，所以只能在家里下载一堆壁纸图片再到学校随机读取切换，没有写网络
    爬虫。在切换壁纸的时候使用了WPF做了一点过渡动画，具体做法是在系统壁纸的window上覆盖
    了一层自己的窗口，在 Module.Wallpaper.WallSwitchEffect 中可以找到代码细节。

3. 晨读时播放英语听力 (Module.AutoPlay)
    使用了 NAudio(Nuget包) 中的 AudioEndpointVolume 使播放听力前系统音量调节至50%。

- 致谢

感谢 Stack Overflow 提供技术支持（逃