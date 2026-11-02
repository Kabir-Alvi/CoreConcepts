// ==============================================================================
// CoreConcepts (CC) Enterprise Application
// Architected & Developed by: Kabir Alvi
// GitHub: https://github.com/Kabir-Alvi
// Built with ASP.NET Core (.NET 10), EF Core, and Microsoft SQL Server
// ==============================================================================

(function () {
    const bannerStyle = [
        'color: #38bdf8',
        'background: #090d16',
        'font-size: 14px',
        'font-family: monospace',
        'font-weight: 900',
        'padding: 12px 18px',
        'border-radius: 8px',
        'border: 1px solid #4f46e5',
        'line-height: 1.3',
        'text-shadow: 0 0 10px rgba(56, 189, 248, 0.5)'
    ].join(';');

    const titleStyle = [
        'color: #ffffff',
        'background: linear-gradient(135deg, #4f46e5, #06b6d4)',
        'font-size: 15px',
        'font-weight: 800',
        'padding: 6px 14px',
        'border-radius: 6px',
        'text-shadow: 0 1px 3px rgba(0,0,0,0.4)'
    ].join(';');

    const linkStyle = [
        'color: #38bdf8',
        'font-weight: bold',
        'font-size: 12px'
    ].join(';');

    // Bold, Clean ASCII Art for ALVI
    const alviAscii = `
     █████╗ ██╗     ██╗   ██╗██╗
    ██╔══██╗██║     ██║   ██║██║
    ███████║██║     ██║   ██║██║
    ██╔══██║██║     ╚██╗ ██╔╝██║
    ██║  ██║███████╗ ╚████╔╝ ██║
    ╚═╝  ╚═╝╚══════╝  ╚═══╝  ╚═╝
    `;

    console.log(`%c${alviAscii}`, bannerStyle);
    console.log('%c >> CoreConcepts (CC) Enterprise Platform v1.0 << ', titleStyle);
    console.log('%c \n 🚀 System Architect & Lead Engineer: Kabir Alvi\n 🔗 GitHub: https://github.com/Kabir-Alvi\n 💻 Tech Stack: ASP.NET Core (.NET 10) | Entity Framework Core | Microsoft SQL Server 2022\n 🌐 Ready for Production Deployment.\n', 'color: #94a3b8; font-size: 12px; font-family: monospace;');
    console.log('%cExplore the code and enjoy building! Have inquiries? Reach out via GitHub.', linkStyle);
})();
