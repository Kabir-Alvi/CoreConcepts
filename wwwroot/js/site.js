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
        'font-size: 13px',
        'font-family: monospace',
        'font-weight: bold',
        'padding: 10px 14px',
        'border-radius: 8px',
        'border: 1px solid #4f46e5',
        'line-height: 1.5'
    ].join(';');

    const titleStyle = [
        'color: #ffffff',
        'background: linear-gradient(135deg, #4f46e5, #06b6d4)',
        'font-size: 16px',
        'font-weight: 800',
        'padding: 6px 12px',
        'border-radius: 6px',
        'text-shadow: 0 1px 3px rgba(0,0,0,0.4)'
    ].join(';');

    const linkStyle = [
        'color: #38bdf8',
        'font-weight: bold',
        'font-size: 12px'
    ].join(';');

    const asciiLogo = `
   ____                  ____                            _       
  / ___|___  _ __ ___   / ___|___  _ __   ___ ___ _ __ | |_ ___ 
 | |   / _ \\| '__/ _ \\ | |   / _ \\| '_ \\ / __/ _ \\ '_ \\| __/ __|
 | |__| (_) | | |  __/ | |__| (_) | | | | (_|  __/ |_) | |_\\__ \\
  \\____\\___/|_|  \\___|  \\____\\___/|_| |_|\\___\\___| .__/ \\__|___/
                                                 |_|            
    `;

    console.log(`%c${asciiLogo}`, bannerStyle);
    console.log('%c >> CoreConcepts (CC) Enterprise Platform v1.0 << ', titleStyle);
    console.log('%c \n 🚀 System Architect & Lead Engineer: Kabir Alvi\n 🔗 GitHub: https://github.com/Kabir-Alvi\n 💻 Tech Stack: ASP.NET Core (.NET 10) | Entity Framework Core | Microsoft SQL Server 2022\n 🌐 Ready for Production Deployment.\n', 'color: #94a3b8; font-size: 12px; font-family: monospace;');
    console.log('%cExplore the code and enjoy building! Have inquiries? Reach out via GitHub.', linkStyle);
})();
